using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Services.Interface;
using System;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountsController : Controller
    {
        private IIdentityService _identityService;
        private IRequestCookieCollection _cookies; 

        public AccountsController(IIdentityService identityService, IHttpContextAccessor httpContext)
        {
            _identityService = identityService;
            _cookies = httpContext.HttpContext.Request.Cookies;
        }

        /// <summary>
        /// Registers a user account and returns JWT token on success.
        /// </summary>
        /// <param name="createAccountDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto createAccountDto)
        {
            Users.VerifyCreateDto(createAccountDto);

            var authenticationResponse = await _identityService.RegisterUserAsync(createAccountDto);

            if (!String.IsNullOrEmpty(authenticationResponse.Error))
            {
                return BadRequest(authenticationResponse.Error);
            }

            return Ok(authenticationResponse); 
        }

        /// <summary>
        /// Verifies users login credentials and returns JWT token on success. 
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            Users.VerifyLoginDto(loginDto);

            var authenticationResponse = await _identityService.LoginAsync(loginDto); 

            if (!String.IsNullOrEmpty(authenticationResponse.Error))
            {
                return BadRequest(authenticationResponse.Error);
            }

            return Ok(authenticationResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshRequest)
        {
            var refreshToken = _cookies["refreshToken"];
            var authenticationDto = await _identityService.RefreshTokenAsync(refreshRequest.Token, refreshToken);

            if (!String.IsNullOrEmpty(authenticationDto.Error))
            {
                return BadRequest(authenticationDto.Error);
            }

            return Ok(authenticationDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updatePasswordDto"></param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto)
        {
            if (updatePasswordDto.NewPassword != updatePasswordDto.ConfirmPassword)
            {
                return BadRequest("Confirm passowrd must match new passowrd.");
            }

            if (updatePasswordDto.NewPassword.Length < 7)
            {
                return BadRequest("Password must be at least 7 characters.");
            }

            var isUpdated = await _identityService.UpdatePassword(updatePasswordDto);

            if (!isUpdated)
            {
                return BadRequest("Failed to update password");
            }

            var refreshToken = _cookies["refreshToken"];

            var authenticationDto = await _identityService.RefreshTokenAsync(updatePasswordDto.Token, refreshToken);

            if (!String.IsNullOrEmpty(authenticationDto.Error))
            {
                return BadRequest(authenticationDto.Error);
            }

            return Ok(authenticationDto);
        }
    }
}
