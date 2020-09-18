using Microsoft.AspNetCore.Identity;
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
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly UnitOfWork _unitOfWork; 

        public IdentityService(
            UserManager<Users> userManager, 
            TokenValidationParameters tokenValidationParameters, 
            MovieFinderContext movieFinderContext)
        {
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        public async Task<RegisterDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);

            if (existingUser != null)
            {
                return new RegisterDto
                {
                    IsSuccess = false,
                    Error = "User with this email already exists"
                };
            }

            var newUser = new Users(registerDto);

            var createdUser = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!createdUser.Succeeded)
            {
                return new RegisterDto
                {
                    IsSuccess = false,
                    Error = string.Join("\n", createdUser.Errors.Select(e => e.Description))
                };
            }

            return registerDto;
        }

        public async Task<AuthenticationDto> LoginAsync(LoginDto loginDto)
        {
            Console.WriteLine("**** In Login Async");

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            Console.WriteLine("**** User found: " + user);

            if (user == null)
            {
                return new AuthenticationDto
                {
                    IsSuccess = false,
                    Error = "User login failed."
                };
            }

            var verifedPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!verifedPassword)
            {
                return new AuthenticationDto
                {
                    IsSuccess = false,
                    Error = "User login failed."
                };
            }

            var jwtToken = CreateJwtToken(user);

            return new AuthenticationDto
            {
                Token = jwtToken,
                IsSuccess = true,
                UserDto = new UsersDto(user)
            };
        }

        public AuthenticationDto GuestLogin()
        {
            var guestUser = new Users()
            {
                UserId = 0,
                FirstName = "Guest",
                LastName = "Account",
                Email = "GuestAccount@guest.com",
                Id = Guid.NewGuid().ToString()
            };

            var jwtToken = CreateJwtToken(guestUser);

            return new AuthenticationDto
            {
                Token = jwtToken,
                UserDto = new UsersDto(guestUser)
            };
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

        public string CreateJwtToken(Users user)
        {
            Console.WriteLine("**** User logging in: " + user);

            Console.WriteLine("**** jwt secret: " + MoviePrestoSettings.Configuration["JwtSecret"]);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(MoviePrestoSettings.Configuration["JwtSecret"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("UserId", user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(MoviePrestoSettings.Configuration["TokenLifetime"])),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public RefreshToken CreateRefreshToken(string jwtToken, int userId)
        {
            if (jwtToken == null)
            {
                throw new ArgumentException("JwtToken required to create RefreshToken.");
            }

            if (userId < 0)
            {
                throw new ArgumentException("UserId must be greater than zero.");
            }

            var refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.UtcNow.AddMonths(int.Parse(MoviePrestoSettings.Configuration["RefreshLifetime"])),
                UserId = userId
            };

            return refreshToken;
        }

        public bool RefreshTokenIsValid(string jwtToken, string refreshToken)
        {
            var isValid = TokenIsValid(jwtToken); 
            var storedRefreshToken = _unitOfWork.RefreshToken.GetByToken(refreshToken);
            try
            {
                if (!isValid)
                {
                    throw new Exception("Refresh token not valid");
                }
                if (storedRefreshToken == null)
                {
                    throw new Exception("Refresh token not found");
                }

                if (DateTime.UtcNow > storedRefreshToken.ExpirationDate)
                {
                    throw new Exception("Refresh token has expired");
                }

                if (storedRefreshToken.Invalidated)
                {
                    throw new Exception("Refresh token has been invalidated.");
                }

                return true;
            }
            
            catch(Exception e)
            {
                Console.WriteLine("Error validating refresh token: " + e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Validate jwtToken is a valid token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool TokenIsValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(MoviePrestoSettings.Configuration["JwtSecret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
                var princpal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken); 
                if (!IsJwtWithValidSecurityAlgorithim(validatedToken))
                {
                    return false;
                }
                return true;

            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Helper function that validates Jwt token was created with valid security algorithim.
        /// </summary>
        /// <param name="validatedToken"></param>
        /// <returns></returns>
        private bool IsJwtWithValidSecurityAlgorithim(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
