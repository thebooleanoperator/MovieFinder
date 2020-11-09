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

        public UserSearchHistoryController(
            MovieFinderContext movieFinderContext, 
            IHttpContextAccessor httpContextAccessor)
        {
            _sessionVars = new Session(httpContextAccessor.HttpContext.User);
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        /// <summary>
        /// Creates a new UserSearchHistory entity.
        /// </summary>
        /// <param name="userSearchHistoryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] UserSearchHistoryDto userSearchHistoryDto)
        {
            if (userSearchHistoryDto == null)
            {
                return BadRequest("User search dto is required.");
            }

            var userSearchHistory = new UserSearchHistory(userSearchHistoryDto, _sessionVars.UserId);

            _unitOfWork.UserSearchHistory.Add(userSearchHistory);
            
            var existingHistory = _unitOfWork.UserSearchHistory
                .GetByMovieId(_sessionVars.UserId, userSearchHistory.MovieId);

            if (existingHistory != null)
            {
                existingHistory.IsDeleted = true;
                _unitOfWork.UserSearchHistory.Update(existingHistory); 
            }

            _unitOfWork.SaveChanges();

            return Ok(userSearchHistory);
        }

        /// <summary>
        /// Gets all of the userSearchHistorys that belong to a userId. 
        /// Can filter by using query param historyLength.
        /// </summary>
        /// <param name="historyLength"></param>
        /// <returns></returns>
        [HttpGet("{skip?}/{count?}")]
        [Authorize]
        public IActionResult GetAll(int? skip, int? count)
        {
            var userSearchHistorys = _unitOfWork.UserSearchHistory.
                GetAllByUserId(_sessionVars.UserId, skip, count).ToList(); 

            if (userSearchHistorys == null || userSearchHistorys.Count() == 0)
            {
                return NoContent();
            }

            return Ok(userSearchHistorys);
        }
    }
}
