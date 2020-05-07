using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System.Linq;

namespace MovieFinder.Controllers
{
    [Authorize]
    [Route("{controller}")]
    public class LikedMoviesController : Controller 
    {
        private UnitOfWork _unitOfWork;
        private Session _session;
        private IMoviesService _moviesService; 

        public LikedMoviesController(MovieFinderContext movieFinderContext, IHttpContextAccessor httpContext, IMoviesService moviesService)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _session = new Session(httpContext.HttpContext.User);
            _moviesService = moviesService;
        }

        /// <summary>
        /// Creates a likedMovie.
        /// </summary>
        /// <param name="likedMoviesDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] LikedMoviesDto likedMoviesDto)
        { 
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

        /// <summary>
        /// Gets all of a users likedMovies by userId.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            var likedMovies =_unitOfWork.LikedMovies.GetAllByUserId(_session.UserId).ToList();

            var completeLikedMovies = _moviesService.GetCompleteLikedMovies(likedMovies);

            if (completeLikedMovies == null || completeLikedMovies.Count() == 0)
            {
                return NoContent();
            }

            return Ok(completeLikedMovies); 
        }
    }
}
