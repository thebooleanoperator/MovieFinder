using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountsController : Controller
    {
        private IIdentityService _identityService;
        private IRequestCookieCollection _cookies;
        private UnitOfWork _unitOfWork;
        private Session _sesionVars; 

        public AccountsController(IIdentityService identityService, IHttpContextAccessor httpContext, MovieFinderContext movieFinderContext)
        {
            _identityService = identityService;
            _cookies = httpContext.HttpContext.Request.Cookies;
            _unitOfWork = new UnitOfWork(movieFinderContext);

            if(httpContext.HttpContext.User != null)
            {
                _sesionVars = new Session(httpContext.HttpContext.User);
            }
        }

        /// <summary>
        /// Registers a user account.
        /// </summary>
        /// <param name="createAccountDto"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var register = await _identityService.RegisterAsync(registerDto);

            if (!register.IsSuccess)
            {
                return BadRequest(register.Error);
            }

            return Ok(); 
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
            var authenticationDto = await _identityService.LoginAsync(loginDto); 

            // If error is not null, return the error to client. 
            if (!authenticationDto.IsSuccess)
            {
                return BadRequest(authenticationDto.Error);
            }

            // Create refresh token and add to http only cookie.
            var refreshToken = _identityService.CreateRefreshToken(authenticationDto.Token, authenticationDto.UserDto.UserId);

            _unitOfWork.RefreshToken.Add(refreshToken);
            _unitOfWork.SaveChanges();

            /*HttpContext.Response.Cookies.Append("Cookie", refreshToken.Token, new CookieOptions()
            {
                HttpOnly = true
            });*/

            return Ok(authenticationDto);
        }

        /// <summary>
        /// Logs in a user with a guest account. 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult GuestLogin()
        {
            var authenticationResponse = _identityService.GuestLogin();

            // If error is not null, return the error to client.
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
        public IActionResult RefreshToken([FromBody] RefreshTokenRequestDto refreshRequest)
        {
            var refreshToken = _cookies["refreshToken"];
            var refreshTokneIsValid = _identityService.RefreshTokenIsValid(refreshRequest.Token, refreshToken);

            if (!refreshTokneIsValid)
            {
                return BadRequest("Refresh token is not valid");
            }

            var user = _unitOfWork.Users.GetByUserId(_sesionVars.UserId);
            var jwtToken = _identityService.CreateJwtToken(user);

            var authenticationDto =  new AuthenticationDto
            {
                Token = jwtToken,
                UserDto = new UsersDto(user)
            };

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

            /*var refreshToken = _cookies["refreshToken"];

            var authenticationDto = await _identityService.RefreshTokenAsync(updatePasswordDto.Token, refreshToken);

            if (!String.IsNullOrEmpty(authenticationDto.Error))
            {
                return BadRequest(authenticationDto.Error);
            }

            return Ok(authenticationDto);*/

            return Ok();
        }
    }
}
