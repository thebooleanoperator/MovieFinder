﻿using Microsoft.AspNetCore.Identity;
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
        /// Registers a user by taking in a CreateAccountDto.
        /// </summary>
        /// <param name="createAccountDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto createAccountDto)
        {
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
        /// Checks email and password match credentials saved in ASPNetUsers. 
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        /*[HttpPost]
        public async Task<bool> Login([FromBody]LoginDto loginDto)
        {
            if (loginDto == null)
            {
                return false;
            }

            var isValid = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false); 

            if (!isValid.Succeeded)
            {
                return false;
            }

            return true;
        }*/
    }
}
