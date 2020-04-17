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
    
    [Route("[controller]/[action]")]
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
        public async Task<IActionResult> CreateMovieFromImdbId([FromBody] ImdbIdDto imdbIdDto)
        {
            if (imdbIdDto == null)
            {
                return BadRequest("ImdbIdDto must not be null.");
            }

            var imdbId = _unitOfWork.ImdbIds.Get(imdbIdDto.ImdbId); 

            if (imdbId == null)
            {
                return BadRequest("ImdbId does not exist.");
            }

            var imdbInfo = await _moviesService.GetImdbMovieInfo(imdbId);

            if (imdbInfo == null)
            {
                return NotFound("Parsing movie info failed.");
            }

            var existingMovie = _unitOfWork.Movies.GetByImdbId(imdbInfo.ImdbId);

            // Don't create a duplicate Movie.
            if (existingMovie != null)
            {
                return Ok(existingMovie);
            }

            var movie = new Movies(imdbInfo, imdbId);
            var streamingDataDto = await _moviesService.GetStreamingData(movie.Title);

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges();

            // Creates Synposis, Genres, and StreamingData table asscoiated with movie created.
            FillAssociatedTables(imdbInfo, movie, streamingDataDto);

            return Ok(movie);
        }

        /// <summary>
        /// Endpoint used to create Movies from MovieTitlesDto. Used to manually add movies to staff reccomendations.
        /// </summary>
        /// <param name="movieInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAllMoviesWithTitle([FromBody] MovieTitlesDto movieInfo)
        {
            var imdbIds = await _moviesService.GetImdbIdsFromTitle(movieInfo.MovieTitle, movieInfo.Year);
            if (imdbIds == null)
            {
                return BadRequest("Could not find imdbId.");
            }
            var movies = new List<Movies>();
            foreach (var imdbId in imdbIds)
            {
                var imdbInfo = await _moviesService.GetImdbMovieInfo(imdbId);
                // If the imdbInfo is null, parse failed. Continue iteration.
                if (imdbInfo == null) {continue;}

                imdbInfo.IsRec = movieInfo.IsRec;

                var existingMovie = _unitOfWork.Movies.GetByImdbId(imdbInfo.ImdbId);

                // Don't save a dupe Movie.
                if (existingMovie != null)
                {
                    movies.Add(existingMovie);
                }
                else
                {
                    var movie = new Movies(imdbInfo, imdbId);
                    _unitOfWork.Movies.Add(movie);
                    _unitOfWork.SaveChanges();
                    var streamingDataDto = await _moviesService.GetStreamingData(movie.Title);

                    // Creates Synposis, Genres, and StreamingData table asscoiated with movie created.
                    FillAssociatedTables(imdbInfo, movie, streamingDataDto);
                    movies.Add(movie);
                }
            }
            return Ok(movies);
        }
        
        /// <summary>
        /// Gets a movie, streamingData, and Synopsis from ImdbId.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetFromImdbId(string id)
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

        [HttpGet]
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
        public void FillAssociatedTables(ImdbInfoDto imdbInfo, Movies movie, StreamingDataDto streamingDataDto, bool saveTables = true)
        {
            var streamingData = new StreamingData(streamingDataDto, movie);
            _unitOfWork.StreamingData.Add(streamingData);

            var synopsis = new Synopsis(imdbInfo, movie);
            _unitOfWork.Synopsis.Add(synopsis);

            var genres = new Genres(imdbInfo, movie);
            _unitOfWork.Genres.Add(genres);

            if (saveTables)
            {
                _unitOfWork.SaveChanges();
            }
        }
    }
}
