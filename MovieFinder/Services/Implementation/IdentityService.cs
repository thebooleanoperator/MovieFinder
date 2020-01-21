using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieFinder.DtoModels;
using MovieFinder.Models;
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

        public IdentityService(UserManager<Users> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
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
                    Error = createdUser.Errors.Select(x => x.Description).ToString()
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
                    Error = "User does not exist."
                };
            }

            var verifedPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password); 

            if (!verifedPassword)
            {
                return new AuthenticationDto
                {
                    Error = "User/Password combination failed."
                };
            }

            return AuthenticationResult(user); 
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
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id),
                    new Claim("UserId", newUser.UserId.ToString())
                }),
                Expires = DateTime.Now.AddHours(12),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationDto
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                UserId = newUser.UserId
            };
        }
    }
}
