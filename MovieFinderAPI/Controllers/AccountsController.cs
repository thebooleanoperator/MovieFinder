using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountsController : Controller
    {
        private IIdentityService _identityService;
        private ITokenService _tokenService;
        private UnitOfWork _unitOfWork;
        private Session _sesionVars; 

        public AccountsController(
            IIdentityService identityService, 
            ITokenService tokenService,
            IHttpContextAccessor httpContext, 
            MovieFinderContext movieFinderContext)
        {
            _identityService = identityService;
            _tokenService = tokenService;
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        /// <summary>
        /// Registers a new user account.
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

            var refreshToken = _unitOfWork.RefreshToken.GetByUserId(authenticationDto.UserDto.UserId);
            // Create refresh token and add to http only cookie if one doesnt exist.
            if (refreshToken == null)
            {
                refreshToken = _tokenService.CreateRefreshToken(authenticationDto.Token, authenticationDto.UserDto.UserId);
                _unitOfWork.RefreshToken.Add(refreshToken);
                _unitOfWork.SaveChanges();
            }
          
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
            var authenticationDto = _identityService.GuestLogin();

            // If error is not null, return the error to client.
            if (!authenticationDto.IsSuccess)
            {
                return BadRequest(authenticationDto.Error);
            }

            var refreshToken = _unitOfWork.RefreshToken.GetByUserId(authenticationDto.UserDto.UserId);
            // Create refresh token and add to http only cookie if one doesnt exist.
            if (refreshToken == null)
            {
                refreshToken = _tokenService.CreateRefreshToken(authenticationDto.Token, authenticationDto.UserDto.UserId);
                _unitOfWork.RefreshToken.Add(refreshToken);
                _unitOfWork.SaveChanges();
            }

            return Ok(authenticationDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshRequest"></param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        [AllowAnonymous]
        public IActionResult RefreshToken(int userId)
        {
            // ToDo: get userId from session. Need to figure out how to only create session when user is logged in.
            var refreshToken = _unitOfWork.RefreshToken.GetByUserId(userId);
            var refreshTokneIsValid = _tokenService.RefreshTokenIsValid(refreshToken);

            if (!refreshTokneIsValid)
            {
                return BadRequest("Refresh token is not valid");
            }
            // Create a new guest account to create token
            // ToDo: Store guest account in AspNetUsers.
            var user = userId == 0 
                ? _identityService.CreateGuest()
                : _unitOfWork.Users.GetByUserId(userId);

            var jwtToken = _tokenService.CreateJwtToken(user);

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
