using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;

namespace MovieFinder.Controllers
{
    [Authorize]
    [Route("{controller}")]
    public class LikedMoviesController : Controller 
    {
        private UnitOfWork _unitOfWork;
        private Session _session; 

        public LikedMoviesController(MovieFinderContext movieFinderContext, IHttpContextAccessor httpContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _session = new Session(httpContext.HttpContext.User); 
        }

        [HttpPost]
        public IActionResult Create([FromBody] LikedMoviesDto likedMoviesDto)
        { 
            //Need to implement Users to make sure we are connecting an existing user. 
            var user = _unitOfWork.Users.Get(likedMoviesDto.UserId); 

            if(user == null)
            {
                return NotFound(); 
            }

            var movie = _unitOfWork.Movies.Get(likedMoviesDto.MovieId);

            if(movie == null)
            {
                return NotFound(); 
            }

            var likedMovie = new LikedMovies(likedMoviesDto);

            _unitOfWork.LikedMovies.Add(likedMovie);
            _unitOfWork.SaveChanges();

            return Ok(likedMovie);
        }

        [HttpGet]
        public IActionResult GetAllByUserId()
        {
            //Need to implement Users to make sure we are connecting an existing user. 
            var user = _unitOfWork.Users.GetByUserId(_session.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var likedMovies =_unitOfWork.LikedMovies.GetAll(_session.UserId);

            if (likedMovies.Count == 0)
            {
                return NoContent();
            }
            return Ok(likedMovies); 
        }
    }
}
