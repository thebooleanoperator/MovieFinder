using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Implementation;
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
        private readonly UnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public IdentityService(
            UserManager<Users> userManager, 
            MovieFinderContext movieFinderContext,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _tokenService = tokenService;
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

            registerDto.IsSuccess = true;
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

            var jwtToken = _tokenService.CreateJwtToken(user);

            if (!_tokenService.JwtTokenIsValid(jwtToken))
            {
                return new AuthenticationDto
                {
                    IsSuccess = false,
                    Error = "Error creating Jwt Token."
                };
            }

            return new AuthenticationDto
            {
                Token = jwtToken,
                IsSuccess = true,
                UserDto = new UsersDto(user)
            };
        }

        public AuthenticationDto GuestLogin()
        {
            var guestUser = CreateGuest();

            var jwtToken = _tokenService.CreateJwtToken(guestUser);

            if (!_tokenService.JwtTokenIsValid(jwtToken))
            {
                return new AuthenticationDto
                {
                    IsSuccess = false,
                    Error = "Error creating Jwt Token."
                };
            }

            return new AuthenticationDto
            {
                Token = jwtToken,
                IsSuccess = true,
                UserDto = new UsersDto(guestUser)
            };
        }

        // ToDo: look into creating guest row in AspNetUsers.
        public Users CreateGuest()
        {
            return new Users()
            {
                UserId = 0,
                FirstName = "Guest",
                LastName = "Account",
                Email = "GuestAccount@guest.com",
                Id = Guid.NewGuid().ToString()
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
    }
}
