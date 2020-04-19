using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        private UnitOfWork _unitOfWork;
        private IMoviesService _moviesService; 

        public MoviesController(MovieFinderContext movieFinderContext, IMoviesService moviesService)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _moviesService = moviesService;
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
                return BadRequest("ImdbIdDto must not be null.");
            }

            var imdbId = _unitOfWork.ImdbIds.Get(moviesDto.ImdbId); 

            var rapidMovieData = await _moviesService.GetMovieInfo(imdbId);

            if (rapidMovieData == null)
            {
                return NotFound("Parsing movie info failed.");
            }

            var existingMovie = _unitOfWork.Movies.GetByImdbId(rapidMovieData.ImdbId);

            // Don't create a duplicate Movie.
            if (existingMovie != null)
            {
                return Ok(existingMovie);
            }

            var movie = new Movies(rapidMovieData, imdbId);
            var rapidStreamingData = await _moviesService.GetStreamingData(movie.Title, movie.ImdbId);

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges();

            // Creates Synposis, Genres, and StreamingData table asscoiated with movie created.
            FillAssociatedTables(rapidMovieData, movie, rapidStreamingData);

            return Ok(movie);
        }

        /// <summary>
        /// Gets a movie, streamingData, and Synopsis from ImdbId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetByImdbId(string id)
        {
            if (id == null)
            {
                return BadRequest("Id must not be null.");
            }

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
            // Get Streaming Data, Synopsis, and Genres to return all movie info.
            var streamingData = _unitOfWork.StreamingData.GetByMovieId(movie.MovieId);
            var synopsis = _unitOfWork.Synopsis.GetByMovieId(movie.MovieId);
            var genres = _unitOfWork.Genres.GetByMovieId(movie.MovieId);

            var moviesDto = new MoviesDto(movie, genres, streamingData, synopsis);

            return Ok(moviesDto); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Recommended")]
        [Authorize]
        public IActionResult GetRecommended()
        {
            var recommendedMovies = _unitOfWork.Movies.GetAllRecommended(); 

            if (recommendedMovies == null)
            {
                return BadRequest();
            }

            var recDtos = new List<MoviesDto>();

            foreach(var movie in recommendedMovies)
            {
                var genres = _unitOfWork.Genres.GetByMovieId(movie.MovieId);
                var streamingData = _unitOfWork.StreamingData.GetByMovieId(movie.MovieId);
                var synopsis = _unitOfWork.Synopsis.GetByMovieId(movie.MovieId); 
                var movieDto = new MoviesDto(movie, genres, streamingData, synopsis);
                recDtos.Add(movieDto);
            }

            return Ok(recDtos);
        }

        /// <summary>
        /// Endpoint used to update Movies from MoviesDto.
        /// </summary>
        /// <param name="moviesDto"></param>
        /// <returns></returns>
        [HttpPatch]
        [Authorize]
        public IActionResult UpdateRecommendation([FromBody] RecomendationDto recomendationDto)
        {
            var movie = _unitOfWork.Movies.Get(recomendationDto.MovieId); 

            if (movie == null)
            {
                return BadRequest();
            }

            movie.Patch(movie, recomendationDto);
            _unitOfWork.Movies.Update(movie);
            _unitOfWork.SaveChanges();

            return Ok(movie);
        }

        /// <summary>
        /// Helper method to add all movie info to tables at once.
        /// </summary>
        /// <param name="imdbInfo"></param>
        /// <param name="movie"></param>
        /// <param name="streamingDataDto"></param>
        /// <param name="saveTables"></param>
        private void FillAssociatedTables(RapidMovieDto rapidMovieData, Movies movie, RapidStreamingDto rapidStreamingData, bool saveTables = true)
        {
            var streamingData = new StreamingData(rapidStreamingData, movie);
            _unitOfWork.StreamingData.Add(streamingData);

            var synopsis = new Synopsis(rapidMovieData, movie);
            _unitOfWork.Synopsis.Add(synopsis);

            var genres = new Genres(rapidMovieData, movie);
            _unitOfWork.Genres.Add(genres);

            if (saveTables)
            {
                _unitOfWork.SaveChanges();
            }
        }
    }
}
