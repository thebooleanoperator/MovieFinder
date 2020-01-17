using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Services.Interface;
using System;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [AllowAnonymous]
    [Route("[controller]/{action}")]
    public class AccountsController : Controller
    {
        private IIdentityService _identityService; 

        public AccountsController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Registers a user account and creates a JWT token. Returns the token on success.
        /// </summary>
        /// <param name="createAccountDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto createAccountDto)
        {
            Users.VerifyCreateDto(createAccountDto);

            var authenticationResponse = await _identityService.RegisterUserAsync(createAccountDto);

            if (!authenticationResponse.Success)
            {
                return BadRequest(authenticationResponse.Error);
            }

            return Ok(new AuthenticationDto
            {
                Token = authenticationResponse.Token,
                Success = true
            });
        }

        /// <summary>
        /// Verifies users login credentials and returns JWT token on success. 
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            Users.VerifyLoginDto(loginDto);

            var authenticationResponse = await _identityService.LoginAsync(loginDto); 

            if (!authenticationResponse.Success)
            {
                return BadRequest(authenticationResponse.Error);
            }

            return Ok(new AuthenticationDto
            {
                Token = authenticationResponse.Token,
                Success = true
            });
        }
    }
}
