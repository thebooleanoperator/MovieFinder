using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]/{action}")]
    public class AccountsController : Controller
    {
        private SignInManager<Users> _signInManager;
        private UserManager<Users> _userManager;

        public AccountsController(UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto createAccountDto)
        {
            var user = Users.CreateUser(createAccountDto);
            var createdUser = await _userManager.CreateAsync(user, createAccountDto.Password);

            if (!createdUser.Succeeded)
            {
                throw new InvalidOperationException("Unable to register new account");
            }

            await _signInManager.SignInAsync(user, false);
            return Ok();
        }
    }
}
