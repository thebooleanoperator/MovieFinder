using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Services.Interface;
using System;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]/{action}")]
    public class AccountsController : Controller
    {
        private IIdentityService _identityService; 

        public AccountsController(IIdentityService identityService)
        {
            _identityService = identityService;
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshRequest)
        {
            var authenticationResponse = await _identityService.RefreshTokenAsync(refreshRequest);

            if (!String.IsNullOrEmpty(authenticationResponse.Error))
            {
                return BadRequest(authenticationResponse.Error);
            }

            return Ok(authenticationResponse);
        }
    }
}
