using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        public async Task<IActionResult> AddImdbId([FromBody] MovieTitlesDto movieTitles)
        {
            string longurl = "https://movie-database-imdb-alternative.p.rapidapi.com/";
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["r"] = "json";
            query["page"] = "1";
            query["type"] = "movie";
            query["s"] = movieTitles.MovieTitle;
            query["y"] = $"{movieTitles.Year}";
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, longurl);

            request.Headers.Add("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "98148972b0mshcf5fd6487ff6f4ap1b7554jsn9f54713ce432");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);
            
            //Check to make sure response resolved sucuessfully. 
            if (response.IsSuccessStatusCode)
            {
                //Read the string asynchronosouly from response. 
                var responseBodyAsText = await response.Content.ReadAsStringAsync();
                //Parse the string into an object that contains an array of objects. 
                var parsedJson = JObject.Parse(responseBodyAsText);
                //Get the JTokens from the search results. 
                List<JToken> searchResults = parsedJson["Search"].Children().ToList();

                //Iterate through the search results and convert each Jmovie into a Movies object, 
                //then check if the title and year match. Save and break on true. 
                foreach(var Jmovie in searchResults)
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
             else
             {
                 return BadRequest(response.StatusCode);
             }
        }
    }
}
