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

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationDto
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };

        } 
    }
}
