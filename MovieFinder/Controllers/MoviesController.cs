using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        private UnitOfWork _unitOfWork;
        private IHttpClientFactory _clientFactory;

        public MoviesController(MovieFinderContext movieFinderContext, IHttpClientFactory clientFactory)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _clientFactory = clientFactory;
        }

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
            if (existingMovie != null) { return Ok(existingMovie);}

            var movie = new Movies(imdbInfo, imdbId);
            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges();

            //Look into refactoring to avoid two save changes.
            var synopsis = new Synopsis(imdbInfo, movie);
            _unitOfWork.Synopsis.Add(synopsis);

            var genres = new Genres(imdbInfo, movie);
            _unitOfWork.Genres.Add(genres);

            _unitOfWork.SaveChanges();
            return Ok(movie);
        }

        [HttpPatch]
        public async Task<IActionResult> AddMoviesFromImdbIdsTable([FromQuery] int page, [FromQuery] int count)
        {
            var movieTitles= _unitOfWork.MovieTitles.GetNext(page, count);

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

                var movie = new Movies(imdbInfo, imdbId);
                _unitOfWork.Movies.Add(movie);
                _unitOfWork.SaveChanges();

                //Look into refactoring.
                var synopsis = new Synopsis(imdbInfo, movie);
                _unitOfWork.Synopsis.Add(synopsis);

                var genres = new Genres(imdbInfo, movie);
                _unitOfWork.Genres.Add(genres);
            }
            _unitOfWork.SaveChanges();
            return Ok();
        }

        public async Task<ImdbIds> SaveImdbId(string title, int year)
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

        public async Task<ImdbInfoDto> GetImdbMovieInfo([FromBody] ImdbIds imdbId)
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
    }
}
