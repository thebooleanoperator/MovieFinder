﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
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
                return NoContent();
            }

            var imdbId = _unitOfWork.ImdbIds.Get(moviesDto.ImdbId);

            var rapidMovieData = await _moviesService.GetMovieInfo(imdbId);

            if (rapidMovieData.HasError == true)
            {
                return NotFound(rapidMovieData.ErrorMessage);
            }

            var movie = new Movies(rapidMovieData, imdbId);
            var rapidStreamingData = await _streamingDataService.GetStreamingData(movie.ImdbId);

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges();

            // Create StreamingData and Genres. 
            var streamingData = new StreamingData(rapidStreamingData, movie);
            _unitOfWork.StreamingData.Add(streamingData);

            var genres = new Genres(rapidMovieData, movie);
            _unitOfWork.Genres.Add(genres);

            _unitOfWork.SaveChanges();

            var completeMovieDto = await _moviesService.GetCompleteMovie(movie);

            return Ok(completeMovieDto);
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
            // If the movie does not exist return No Content. 
            if (movie == null)
            {
                return NoContent();
            }

            var completeMoviesDto = await _moviesService.GetCompleteMovie(movie);

            return Ok(completeMoviesDto);
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
            // If the movie does not exist return No Content. 
            if (movie == null)
            {
                return NoContent();
            }

            var completeMoviesDto = await _moviesService.GetCompleteMovie(movie);

            return Ok(completeMoviesDto); 
        }

        /// <summary>
        /// Gets the movies that have been reccomended by StreamSpotter staff.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Recommended")]
        [Authorize]
        public async Task<IActionResult> GetRecommended()
        {
            var recommendedMovies = _unitOfWork.Movies.GetAllRecommended(); 

            if (recommendedMovies == null)
            {
                return BadRequest("Failed to get recommended movies.");
            }

            var completeMovieDtos = await _moviesService.GetCompleteMovie(recommendedMovies);

            return Ok(completeMovieDtos);
        }

        /// <summary>
        /// Gets all of a users liked movies.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Favorites/{skip?}/{count?}")]
        [Authorize]
        public async Task<IActionResult> GetFavorites([FromQuery] int? skip = null, int? count = null)
        {
            var likedMovies = _unitOfWork.LikedMovies.GetAllByUserId(_sessionVars.UserId, skip, count);

            var movieIds = likedMovies.Select(x => x.MovieId).ToList();

            var favoriteMovies = _unitOfWork.Movies.Get(movieIds).ToList();

            if (favoriteMovies == null || favoriteMovies.Count == 0)
            {
                return NoContent();
            }

            var completeMovieDtos = await _moviesService.GetCompleteMovie(favoriteMovies);

            return Ok(completeMovieDtos);
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
