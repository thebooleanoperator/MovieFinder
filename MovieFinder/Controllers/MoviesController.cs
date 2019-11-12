using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;
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

            //Look into refactoring.
            var synopsis = new Synopsis(imdbInfo, movie);
            _unitOfWork.Synopsis.Add(synopsis);
            _unitOfWork.SaveChanges();
            return Ok(movie);
        }

        public async Task<ImdbIds> SaveImdbId(string title, int year)
        {
            var request = RapidRequestSender.ImdbIdsRapidRequest(title, $"{year}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response);
            //Get each movie returned from search. 
            var searchResults = parsedJson["Search"].Children().ToList();

            //Iterate through the search results and convert each Jmovie into a Movies object, 
            //then check if the title and year match. Save and break on true. 
            foreach (var Jmovie in searchResults)
            {
                //Get the ImdbId by converting JObject to ImdbId.
                ImdbIds movie = Jmovie.ToObject<ImdbIds>();
     
                var lowerMovieTitle = movie.Title.ToLower();
                var lowerTitle = title.ToLower();
                if (movie.Year == year && (lowerTitle.Contains(lowerMovieTitle) || lowerMovieTitle.Contains(lowerTitle)))
                {
                    var existingMovie = _unitOfWork.ImdbIds.GetByString(movie.ImdbId);
                    //If we already have the id saved, do not save a dupe.
                    if (existingMovie != null) { return existingMovie; }

                    _unitOfWork.ImdbIds.Add(movie);
                    _unitOfWork.SaveChanges();
                    return movie;
                }
            }
            return null;
        }

        public async Task<ImdbInfoDto> GetImdbMovieInfo([FromBody] ImdbIds imdbIdInfo)
        {
            if (imdbIdInfo == null)
            {
                return null;
            }

            var request = RapidRequestSender.ImdbInfoRapidRequest(imdbIdInfo.ImdbId, $"{imdbIdInfo.Year}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var jsonAndResponse = await HttpValidator.ValidateAndParseResponse(response);
            var parsedJson = jsonAndResponse;
            //Get the ImdbInfoDto by converting JObject.
            var infoDto = parsedJson.ToObject<ImdbInfoDto>();
            return infoDto;
        }
    }
}
