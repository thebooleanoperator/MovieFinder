using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;
using Newtonsoft.Json.Linq;
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

            var movie = new Movies(imdbInfo, imdbId);

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges();

            return Ok(movie);
        }

        public async Task<ImdbIds> SaveImdbId(string title, int year)
        {
             
            var request = RapidRequestSender.ImdbIdsRapidRequest(title, $"{year}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            
            //Check to make sure response resolved sucuessfully. 
            if (response.IsSuccessStatusCode)
            {
                //Read the string asynchronosouly from response. 
                var responseBodyAsText = await response.Content.ReadAsStringAsync();
                //Parse the string into an object that contains an array of objects. 
                var parsedJson = JObject.Parse(responseBodyAsText);
               
                //We need to check if the response came back false.
                var movieFound = parsedJson["Response"].Value<bool>();
                if (!movieFound)
                {
                    return null;
                }

                //Get each movie returned from search. 
                var searchResults = parsedJson["Search"].Children().ToList();

                //Iterate through the search results and convert each Jmovie into a Movies object, 
                //then check if the title and year match. Save and break on true. 
                foreach (var Jmovie in searchResults)
                {
                    ImdbIds movie = Jmovie.ToObject<ImdbIds>();
                   if (movie.Year == year)
                   {
                        _unitOfWork.ImdbIds.Add(movie);
                        _unitOfWork.SaveChanges();
                        return movie;
                    }
                }
                return null; 
             }
            //Response did not resolve correclty, return status code in bad request. 
             else
             {
                return null; 
             }
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

            if (response.IsSuccessStatusCode)
            {
                //Read the string asynchronosouly from response. 
                var responseBodyAsText = await response.Content.ReadAsStringAsync();
                //Parse the string into an object that contains an array of objects. 
                var parsedJson = JObject.Parse(responseBodyAsText);

                //We need to check if the response came back false.
                var movieFound = parsedJson["Response"].Value<bool>();
                if (!movieFound)
                {
                    return null;
                }

                var infoDto = parsedJson.ToObject<ImdbInfoDto>();
                return infoDto;

            }
            //Response did not resolve correclty, return status code in bad request. 
            else
            {
                return null;
            }
        }


    }
}
