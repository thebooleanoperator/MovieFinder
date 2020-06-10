using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;
using System.Linq;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class UserSearchHistoryController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly Session _sessionVars; 

        public UserSearchHistoryController(MovieFinderContext movieFinderContext, IHttpContextAccessor httpContextAccessor)
        {
            _sessionVars = new Session(httpContextAccessor.HttpContext.User);
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] UserSearchHistoryDto userSearchHistoryDto)
        {
            var userSearchHistory = new UserSearchHistory(userSearchHistoryDto);

            _unitOfWork.UserSearchHistory.Add(userSearchHistory);
            _unitOfWork.SaveChanges();

            return Ok(userSearchHistory);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var userSearches = _unitOfWork.UserSearchHistory.GetAll(_sessionVars.UserId).ToList();

            if (userSearches == null || userSearches.Count() == 0)
            {
                return NoContent();
            }

            return Ok(userSearches);
        }
    }
}
