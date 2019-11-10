using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class ImdbInfoController : Controller
    {
        private UnitOfWork _unitOfWork;
        private IHttpClientFactory _clientFactory;

        public ImdbInfoController(MovieFinderContext movieFinderContext, IHttpClientFactory clientFactory)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _clientFactory = clientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] MovieTitlesDto movieTitles)
        {
            var imdbIdUrlString = "https://movie-database-imdb-alternative.p.rapidapi.com/"; 
            var request = RapidRequestSender.ImdbIdsRapidApiRequest(imdbIdUrlString, movieTitles.MovieTitle, $"{movieTitles.Year}");

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
                    var errorMessage = parsedJson["Error"].Value<string>(); 
                    return BadRequest(errorMessage);
                }

                //Get each movie returned from search. 
                var searchResults = parsedJson["Search"].Children().ToList();

                //Iterate through the search results and convert each Jmovie into a Movies object, 
                //then check if the title and year match. Save and break on true. 
                foreach (var Jmovie in searchResults)
                {
                    ImdbIds movie = Jmovie.ToObject<ImdbIds>();
                   if (movie.Title == movieTitles.MovieTitle && movie.Year == movieTitles.Year)
                   {
                        _unitOfWork.ImdbIds.Add(movie);
                        _unitOfWork.SaveChanges();
                        return Ok();
                    }
                }
                return NoContent();
             }
            //Response did not resolve correclty, return status code in bad request. 
             else
             {
                 return BadRequest(response.StatusCode);
             }
        }
    }
}
