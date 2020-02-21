using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Utils
{
    public class MoviesService : IMoviesService
    {
        private IHttpClientFactory _clientFactory;
        private UnitOfWork _unitOfWork;
        // Constructor w/ no params so we can inject into controllers if need be.
        public MoviesService()
        {
        }

        public MoviesService(IHttpClientFactory clientFactory, UnitOfWork unitOfWork)
        {
            _clientFactory = clientFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<ImdbIds>> GetImdbIdsFromTitle(string title, int? year)
        {
            var request = RapidRequestSender.ImdbIdsRapidRequest(title, year);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response);

            if (parsedJson == null) { return null; }

            //Get each movie returned from search. 
            var searchResults = parsedJson["Search"].Children().ToList();
            var imdbIds = new List<ImdbIds>();
            //Iterate through the search results and convert each Jmovie into a Movies object, 
            //then check if the title and year match. Save and break on true. 
            foreach (var Jmovie in searchResults)
            {
                //Get the ImdbId by converting JObject to ImdbId.
                ImdbIds imdbId = Jmovie.ToObject<ImdbIds>();

                var lowerMovieTitle = imdbId.Title.ToLower();
                var lowerTitle = title.ToLower();
                if (lowerMovieTitle.Contains(lowerTitle) && (imdbId.Year == year || year == null))
                {
                    //If we already have the id saved, do not save a dupe.
                    var exisitingId = _unitOfWork.ImdbIds.GetByImdbId(imdbId.ImdbId);
                    if (exisitingId != null)
                    {
                        imdbIds.Add(exisitingId);
                    }
                    else
                    {
                        imdbIds.Add(imdbId);
                    }
                }
            }
            if (imdbIds == null || imdbIds.Count() == 0)
            {
                return null;
            }
            return imdbIds;
        }

        /// <summary>
        /// Gets all of the movie info from an ImdbId.
        /// </summary>
        /// <param name="imdbId"></param>
        /// <returns></returns>
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
