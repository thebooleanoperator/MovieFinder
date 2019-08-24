using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;

namespace MovieFinder.Repository.Repo
{
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
            var likedMovie = new LikedMovies(likedMoviesDto);

            var user = _unitOfWork.Users.Get(likedMovie.UserId); 

            if(user == null)
            {
                return NotFound(); 
            }

            var movie = _unitOfWork.Movies.Get(likedMovie.MovieId);

            if(movie == null)
            {
                return NotFound(); 
            }

            _unitOfWork.LikedMovies.Add(likedMovie);
            _unitOfWork.SaveChanges();

            return Ok(likedMovie);
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetAllByUserId(int userId)
        {
            var likedMovies =_unitOfWork.LikedMovies.GetAll(userId);

            return Ok(likedMovies); 
        }
    }
}
