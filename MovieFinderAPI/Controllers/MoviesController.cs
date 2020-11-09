using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        private UnitOfWork _unitOfWork;
        private IMoviesService _moviesService;
        private IStreamingDataService _streamingDataService;
        private Session _sessionVars;

        public MoviesController(MovieFinderContext movieFinderContext, IMoviesService moviesService,
            IStreamingDataService streamingDataService, IHttpContextAccessor httpContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _moviesService = moviesService;
            _streamingDataService = streamingDataService;
            _sessionVars = new Session(httpContext.HttpContext.User);
        }

        /// <summary>
        /// Creates a movie with an ImdbIdDto. This is used by Movie Finder when user clicks movie title.
        /// </summary>
        /// <param name="imdbIdDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] MoviesDto moviesDto)
        {
            if (moviesDto == null)
            {
                return BadRequest("No movie provided.");
            }

            var existingMovie = _unitOfWork.Movies.GetByImdbId(moviesDto.ImdbId);

            // Don't create a movie that exists in db.
            if (existingMovie != null)
            {
                return BadRequest("That movie has already been created.");
            }

            var imdbId = _unitOfWork.ImdbIds.Get(moviesDto.ImdbId);

            if (imdbId == null)
            {
                return BadRequest("That ImdbId does not exist.");
            }

            // Create MovieData
            RapidMovieDto rapidMovieData;
            try
            {
                rapidMovieData = await _moviesService.GetMovieInfo(imdbId);
            }
            catch(Exception e)
            {
                return BadRequest($"Failed to get movie data. {e}");
            }

            if (rapidMovieData.HasError == true)
            {
                return NotFound(rapidMovieData.ErrorMessage);
            }

            // Create StreamingData
            RapidStreamingDto rapidStreamingData;
            try
            {
                rapidStreamingData = await _streamingDataService.GetStreamingData(moviesDto.ImdbId);
            }
            catch(Exception e)
            {
                return BadRequest($"Failed to get streaming data. {e}");
            }

            var genres = new Genres(rapidMovieData);
            var streamingData = new StreamingData(rapidStreamingData);

            _unitOfWork.Genres.Add(genres);
            _unitOfWork.StreamingData.Add(streamingData);
            _unitOfWork.SaveChanges();

            Movies movie;
            try
            {
                movie = new Movies(rapidMovieData, imdbId, genres, streamingData);
            }
            catch
            {
                return BadRequest("Failed to create movie");
            }

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges();

            var completeMovie = _unitOfWork.Movies.Get(movie.MovieId);

            return Ok(completeMovie);
        }

        /// <summary>
        /// Get a movie by movieId.
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpGet("{movieId}")]
        [Authorize]
        public async Task<ActionResult> Get(int movieId)
        {
            var movie = _unitOfWork.Movies.Get(movieId);
             
            if (movie == null)
            {
                return NoContent();
            }

            movie.StreamingData = await _streamingDataService.GetUpdatedStreamingData(movie.StreamingData, movie.ImdbId);

            return Ok(movie);
        }

        /// <summary>
        /// Gets a movie, streamingData, and Synopsis from ImdbId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("ImdbId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetByImdbId(string id)
        {
            var imdbId = _unitOfWork.ImdbIds.Get(id);

            if (imdbId == null)
            {
                return BadRequest("There was no imdbId found with this id.");
            }

            var movie = _unitOfWork.Movies.GetByImdbId(imdbId.ImdbId);
            
            if (movie == null)
            {
                return NoContent();
            }

            movie.StreamingData = await _streamingDataService.GetUpdatedStreamingData(movie.StreamingData, movie.ImdbId);

            return Ok(movie); 
        }

        /// <summary>
        /// Gets the movies that have been flipped to isRec.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Recommended")]
        //[Authorize]
        public async Task<IActionResult> GetRecommended()
        {
            var recommendedMovies = _unitOfWork.Movies.GetAllRecommended(1); 

            if (recommendedMovies == null)
            {
                return BadRequest("Failed to get recommended movies.");
            }

            return Ok(recommendedMovies);
        }

        /// <summary>
        /// Endpoint used to update Movies recommendation bit.
        /// </summary>
        /// <param name="moviesDto"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize]
        public IActionResult Update([FromBody] MoviesDto moviesDto)
        {
            var movie = _unitOfWork.Movies.Get(moviesDto.MovieId); 

            if (movie == null)
            {
                return BadRequest();
            }

            movie.Patch(moviesDto);
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.SaveChanges();

            return Ok(movie);
        }
    }
}
