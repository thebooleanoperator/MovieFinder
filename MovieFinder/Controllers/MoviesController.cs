using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    
    [Route("[controller]/[action]")]
    public class MoviesController : Controller
    {
        private UnitOfWork _unitOfWork;
        private IHttpClientFactory _clientFactory;
        private IMoviesService _moviesService; 

        public MoviesController(MovieFinderContext movieFinderContext, IMoviesService moviesService)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _moviesService = moviesService;
        }

        /// <summary>
        /// Endpoint used to create Movies from MovieTitlesDto.
        /// </summary>
        /// <param name="movieInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieTitlesDto movieInfo)
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
        /// Endpoint used to add a certain count of movies to the database from the imdbIds table. 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        /*[HttpPost("{page}")]
        public async Task<IActionResult> AddMoviesFromImdbIdsTable(int page, [FromQuery] int count)
        {
            var movieTitles = _unitOfWork.MovieTitles.GetNext(page, count);

            foreach (var moveiTitle in movieTitles)
            {
                var imdbIds = await SaveImdbId(moveiTitle.MovieTitle);
                if (imdbIds == null)
                {
                    continue;
                }

                var imdbInfo = await GetImdbMovieInfo(imdbId);

                if (imdbInfo == null)
                {
                    continue;
                }

                var existingMovie = _unitOfWork.Movies.GetByImdbId(imdbInfo.ImdbId);
                if (existingMovie != null) { return Ok(); }

                var movie = new Movies(imdbInfo, imdbId);
                _unitOfWork.Movies.Add(movie);
                _unitOfWork.SaveChanges();

                var streamingDataDto = await GetStreamingData(movie.Title);
                // Creates Synposis, Genres, and StreamingData table asscoiated with movie created.
                // Does not save every iteration.
                FillAssociatedTables(imdbInfo, movie, streamingDataDto, false);
            }
            _unitOfWork.SaveChanges();
            return Ok();
        }*/

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetFromImdbId(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var imdbId = _unitOfWork.ImdbIds.GetByImdbId(id);

            if (imdbId == null)
            {
                return BadRequest();
            }

            var existingMovie = _unitOfWork.Movies.GetByImdbId(imdbId.ImdbId);
            // If the movie exists, get the streaming data and the synopsis and return MovieSearchDto.
            if (existingMovie != null)
            {
                var streamingData = _unitOfWork.StreamingData.GetByMovieId(existingMovie.MovieId);
                var synopsis = _unitOfWork.Synopsis.GetByMovieId(existingMovie.MovieId);
                var movieSearchDto = new MovieSearchDto(existingMovie, streamingData, synopsis); 
                
                return Ok(movieSearchDto);
            }

            var imdbInfo = await _moviesService.GetImdbMovieInfo(imdbId);
            var movie = new Movies(imdbInfo, imdbId);

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges();

            return Ok(movie); 
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
