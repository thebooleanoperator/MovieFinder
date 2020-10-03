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
    [Authorize]
    [Route("[controller]")]
    public class LikedMoviesController : Controller 
    {
        private UnitOfWork _unitOfWork;
        private Session _session;

        public LikedMoviesController(MovieFinderContext movieFinderContext, IHttpContextAccessor httpContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _session = new Session(httpContext.HttpContext.User);
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
            var usersLikedMovies = _unitOfWork.LikedMovies.GetAll(likedMoviesDto.UserId, null, null).ToList();

            if (usersLikedMovies.Any(lm => lm.MovieId == likedMoviesDto.MovieId))
            {
                return BadRequest("Movie is already liked by user");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpGet("{movieId}")]
        public IActionResult GetByMovieId(int movieId)
        {
            var likedMovie = _unitOfWork.LikedMovies.GetByMovieId(movieId, _session.UserId);

            if (likedMovie == null)
            {
                return NoContent();
            }

            return Ok(likedMovie);
        }

        /// <summary>
        /// Gets all of a users likedMovies by userId.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{skip?}/{count?}")]
        [Authorize]
        public IActionResult GetAll(int? skip, int? count)
        {
            var likedMovies =_unitOfWork.LikedMovies.GetAll(_session.UserId, skip, count).ToList();

            if (likedMovies == null || likedMovies.Count() == 0)
            {
                return NoContent();
            }

            return Ok(likedMovies); 
        }

        /// <summary>
        /// Deletes a likedMovie by likedMovieId.
        /// </summary>
        /// <param name="likedMovieId"></param>
        /// <returns></returns>
        [HttpDelete("{movieId}")]
        [Authorize]
        public IActionResult DeleteByMovieId(int movieId)
        {
            var likedMovie = _unitOfWork.LikedMovies.GetByMovieId(movieId, _session.UserId);
            
            if (likedMovie == null)
            {
                return BadRequest("Liked movie does not exist.");
            }

            _unitOfWork.LikedMovies.Delete(likedMovie);
            _unitOfWork.SaveChanges();

            return Ok();
        }
    }
}
