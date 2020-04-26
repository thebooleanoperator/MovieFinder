using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Auth;
using MovieFinder.DtoModels;
using MovieFinder.Repository;
using MovieFinder.Utils;


namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private Session _session;
        private UnitOfWork _unitOfWork;  

        public UsersController(MovieFinderContext movieFinderContext, IHttpContextAccessor httpContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _session = new Session(httpContext.HttpContext.User);
        }

        [Authorize]
        [HttpGet("{userId}")]
        public IActionResult GetByUserId(int userId)
        {
            if (userId == null)
            {
                return BadRequest();
            }

            if (userId <= 0)
            {
                return BadRequest();
            }

            if (!MovieValidator.MatchUser(userId, _session))
            {
                return Unauthorized(); 
            }

            var user = _unitOfWork.Users.GetByUserId(userId);
            var userDto = new UsersDto(user); 

            return Ok(userDto); 
        }

    }
}
