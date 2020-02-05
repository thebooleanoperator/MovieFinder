using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
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

        public MoviesController(MovieFinderContext movieFinderContext, IHttpClientFactory clientFactory)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Endpoint used to create Movies from MovieTitlesDto.
        /// </summary>
        /// <param name="movieInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieTitlesDto movieInfo)
        {
            var imdbId = await SaveImdbId(movieInfo.MovieTitle, movieInfo.Year);
            if (imdbId == null)
            {
                return BadRequest("Could not find imdbId.");
            }

            var imdbInfo = await GetImdbMovieInfo(imdbId);

            var existingMovie = _unitOfWork.Movies.GetByImdbId(imdbInfo.ImdbId);

            //Don't save a dupe, return existing movie.
            if (existingMovie != null) { return Ok(existingMovie); }

            var movie = new Movies(imdbInfo, imdbId, "6546546");
            //_unitOfWork.Movies.Add(movie);
            //_unitOfWork.SaveChanges();

            //Look into refactoring.
            var streamingDataDto = await GetStreamingData(movie.Title);
            var streamingData = new StreamingData(streamingDataDto, movie);
            _unitOfWork.StreamingData.Add(streamingData);

            var synopsis = new Synopsis(imdbInfo, movie);
            _unitOfWork.Synopsis.Add(synopsis);

            var genres = new Genres(imdbInfo, movie);
            _unitOfWork.Genres.Add(genres);

            //_unitOfWork.SaveChanges();
            return Ok(movie);
        }

        /// <summary>
        /// Endpoint used to add a certain count of movies to the database from the imdbIds table. 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost("{page}")]
        public async Task<IActionResult> AddMoviesFromImdbIdsTable(int page, [FromQuery] int count)
        {
            var movieTitles = _unitOfWork.MovieTitles.GetNext(page, count);

            foreach (var moveiTitle in movieTitles)
            {
                var imdbId = await SaveImdbId(moveiTitle.MovieTitle, moveiTitle.Year);
                if (imdbId == null)
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

                var movie = new Movies(imdbInfo, imdbId, "656232");
                //_unitOfWork.Movies.Add(movie);
                //_unitOfWork.SaveChanges();

                //Look into refactoring.
                var streamingDataDto = await GetStreamingData(movie.Title);
                var streamingData = new StreamingData(streamingDataDto, movie);
                _unitOfWork.StreamingData.Add(streamingData); 

                var synopsis = new Synopsis(imdbInfo, movie);
                _unitOfWork.Synopsis.Add(synopsis);

                var genres = new Genres(imdbInfo, movie);
                _unitOfWork.Genres.Add(genres);
            }
            //_unitOfWork.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetMoviesByTitle([FromQuery] string title)
        {
            if (title == null || title.Length == 0)
            {
                return NoContent();
            }

            var movies = _unitOfWork.Movies.GetAllByTitle(title).ToList();
            var moviesDtos = new List<MoviesDto>();

            foreach (var movie in movies)
            {
                var genres = _unitOfWork.Genres.GetByMovieId(movie.MovieId);
                var movieDto = new MoviesDto(movie, genres);
                moviesDtos.Add(movieDto);
            }

            return Ok(moviesDtos);
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
                var movieDto = new MoviesDto(movie, genres);
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

        /////////////////////////////////////////////PRIVATE HELPER FUNCTIONS//////////////////////////////////////////////

        private async Task<ImdbIds> SaveImdbId(string title, int year)
        {
            var request = RapidRequestSender.ImdbIdsRapidRequest(title, $"{year}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response);

            if (parsedJson == null) { return null; }

            //Get each movie returned from search. 
            var searchResults = parsedJson["Search"].Children().ToList();

            //Iterate through the search results and convert each Jmovie into a Movies object, 
            //then check if the title and year match. Save and break on true. 
            foreach (var Jmovie in searchResults)
            {

                //Get the ImdbId by converting JObject to ImdbId.
                ImdbIds imdbId = Jmovie.ToObject<ImdbIds>(); 
     
                var lowerMovieTitle = imdbId.Title.ToLower();
                var lowerTitle = title.ToLower();
                if (imdbId.Year == year && lowerMovieTitle.Contains(lowerTitle))
                {
                    //If we already have the id saved, do not save a dupe.
                    var existingMovie = _unitOfWork.ImdbIds.GetByString(imdbId.ImdbId);
                    if (existingMovie != null) { return existingMovie; }

                    _unitOfWork.ImdbIds.Add(imdbId);
                    _unitOfWork.SaveChanges();
                    return imdbId;
                }
            }
            return null;
        }

        private async Task<ImdbInfoDto> GetImdbMovieInfo([FromBody] ImdbIds imdbId)
        {
            if (imdbId == null)
            {
                return null;
            }

            var request = RapidRequestSender.ImdbInfoRapidRequest(imdbId.ImdbId, $"{imdbId.Year}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var jsonAndResponse = await HttpValidator.ValidateAndParseResponse(response);

            if (jsonAndResponse == null) { return null; }

            var parsedJson = jsonAndResponse;
            //Get the ImdbInfoDto by converting JObject.
            var infoDto = parsedJson.ToObject<ImdbInfoDto>();
            return infoDto;
        }

        /// <summary>
        /// Returns the netflix Id for a movie or null if the movie is not on netflix. 
        /// </summary>
        /// <param name="imdbId"></param>
        /// <returns></returns>
        private async Task<StreamingDataDto> GetStreamingData(string title)
        {
            if (title == null)
            {
                return null; 
            }

            var request = RapidRequestSender.UtellyRapidRequest(title);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedResponse = await HttpValidator.ValidateAndParseUtellyResponse(response);

            if (parsedResponse == null) { return null; }

            //Get the ImdbInfoDto by converting JObject.
            var streamingResults = parsedResponse["results"].Children().ToList();
 
            foreach (var Jdata in streamingResults)
            {
                var streamingData = Jdata.ToObject<StreamingDataDto>();
                //Only return the data if title matches.
                if (streamingData.Name.ToLower() == title.ToLower())
                {
                    return streamingData;
                }
            }
            return null;
        }
    }
}
