﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Settings;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<Users> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UnitOfWork _unitOfWork; 

        public IdentityService(
            UserManager<Users> userManager, 
            JwtSettings jwtSettings, 
            TokenValidationParameters tokenValidationParameters, 
            MovieFinderContext movieFinderContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        public async Task<AuthenticationDto> RegisterUserAsync(CreateAccountDto createAccountDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(createAccountDto.Email);

            if (existingUser != null)
            {
                return new AuthenticationDto
                {
                    Error = "User with this email already exists"
                };
            }

            var newUser = new Users(createAccountDto);

            var createdUser = await _userManager.CreateAsync(newUser, createAccountDto.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationDto
                {
                    Error = string.Join("\n", createdUser.Errors.Select(e => e.Description))
                };
            }

            return AuthenticationResult(newUser);
        }

        public async Task<AuthenticationDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return new AuthenticationDto
                {
                    Error = "User login failed."
                };
            }

            var verifedPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password); 

            if (!verifedPassword)
            {
                return new AuthenticationDto
                {
                    Error = "User login failed."
                };
            }

            return AuthenticationResult(user); 
        }
        
        public async Task<bool> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(updatePasswordDto.Email);

            if (user == null)
            {
                return false;
            }

            var verifedPassword = await _userManager.CheckPasswordAsync(user, updatePasswordDto.OldPassword);

            if (!verifedPassword)
            {
                return false;
            }

            if (String.IsNullOrEmpty(updatePasswordDto.NewPassword))
            {
                return false;
            }

            var newPassword = _userManager.PasswordHasher.HashPassword(user, updatePasswordDto.NewPassword);

            user.PasswordHash = newPassword;

            var passwordUpdated = await _userManager.UpdateAsync(user);

            if (!passwordUpdated.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<AuthenticationDto> RefreshTokenAsync(string jwtToken, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(jwtToken); 

            if (validatedToken == null)
            {
                return new AuthenticationDto() { Error = "Invalid Token" }; 
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = _unitOfWork.RefreshToken.GetByToken(refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationDto() { Error = "Refresh token does not exist." };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpirationDate)
            {
                return new AuthenticationDto() { Error = "Refresh token has expired." };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationDto() { Error = "Refresh token has been invalidated." };
            }

            if (storedRefreshToken.IsUsed)
            {
                return new AuthenticationDto() { Error = "Refresh token has been used." };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationDto() { Error = "JWT does not match refresh token." };
            }

            storedRefreshToken.IsUsed = true;
            _unitOfWork.RefreshToken.Update(storedRefreshToken);
            _unitOfWork.SaveChanges();

            var user = await _userManager.FindByIdAsync(validatedToken.FindFirst(claim => claim.Type == "Id").Value);
            return AuthenticationResult(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
                var princpal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken); 
                if (!IsJwtWithValidSecurityAlgorithim(validatedToken))
                {
                    return null;
                }
                return princpal;

            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithim(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase));
        }

        private AuthenticationDto AuthenticationResult(Users newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("Id", newUser.Id.ToString()),
                    new Claim("UserId", newUser.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                ExpirationDate = DateTime.UtcNow.AddMonths(_jwtSettings.RefreshLifetime),
                IsUsed = false,
                UserId = newUser.UserId
            };

            _unitOfWork.RefreshToken.Add(refreshToken);
            _unitOfWork.SaveChanges();

            return new AuthenticationDto
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token,
                UserDto = new UsersDto(newUser)
            };
        }
    }
}
