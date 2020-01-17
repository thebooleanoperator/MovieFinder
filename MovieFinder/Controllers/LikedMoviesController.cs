using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;

namespace MovieFinder.Controllers
{
    [Authorize]
    [Route("{controller}")]
    public class LikedMoviesController : Controller 
    {
        private UnitOfWork _unitOfWork; 

        public LikedMoviesController(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext); 
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
        [Route("{userId}")]
        public IActionResult GetAllByUserId(int userId)
        {
            //Need to implement Users to make sure we are connecting an existing user. 
            var user = _unitOfWork.Users.GetByUserId(userId);

            if (user == null)
            {
                return NotFound();
            }

            var likedMovies =_unitOfWork.LikedMovies.GetAll(userId);

            if (likedMovies.Count == 0)
            {
                return NoContent();
            }
            return Ok(likedMovies); 
        }
    }
}
