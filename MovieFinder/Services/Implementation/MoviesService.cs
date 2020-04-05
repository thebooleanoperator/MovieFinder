using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
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

        public MoviesService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
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
                try
                {
                    //Get the ImdbId by converting JObject to ImdbId.
                    ImdbIds imdbId = Jmovie.ToObject<ImdbIds>();

                    var lowerMovieTitle = imdbId.Title.ToLower();
                    var lowerTitle = title.ToLower();
                    if (lowerMovieTitle.Contains(lowerTitle) && (imdbId.Year == year || year == null))
                    {
                        //If we already have the id saved, do not save a dupe.
                        imdbIds.Add(imdbId);
                    }
                }
                catch
                {
                    continue;
                }

            }

            return imdbIds;
        }

        public async Task<List<IdsDto>> GetIdsFromTitle(string title)
        {
            var request = RapidRequestSender.IdsRapidRequest(title);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedJson = await HttpValidator.ValidateAndParseIdResponse(response);

            if (parsedJson == null) { return null; }

            //Get each movie returned from search. 
            var searchResults = parsedJson["titles"].Children().ToList();
            var idsDtos = new List<IdsDto>();
            //Iterate through the search results and convert each Jmovie into a Movies object, 
            //then check if the title and year match. Save and break on true. 
            foreach (var Jmovie in searchResults)
            {
                try
                {
                    IdsDto idDto = Jmovie.ToObject<IdsDto>();
                    idsDtos.Add(idDto);
                }
                catch
                {
                    continue;
                }
            }

            return idsDtos;
        }

        /// <summary>
        /// Gets an imdbId objcet by an imdbId id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ImdbIds> GetImdbIdById(string id)
        {
            if (id == null)
            {
                return null;
            }

            var request = RapidRequestSender.ImdbInfoRapidRequest(id);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response);

            if (parsedJson == null) { return null; }

            //Get the ImdbInfoDto by converting JObject.
            try
            {
                return parsedJson.ToObject<ImdbIds>();
            }
            catch
            {
                return null;
            }
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
            // If parse fails, return null.
            try
            {
                //Get the ImdbInfoDto by converting JObject.
                return parsedJson.ToObject<ImdbInfoDto>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the stream services a movie is streaming on by title.
        /// </summary>
        /// <param name="imdbId"></param>
        /// <returns></returns>
        public async Task<StreamingDataDto> GetStreamingData(string title)
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
                try
                {
                    var streamingData = Jdata.ToObject<StreamingDataDto>();
                    //Only return the data if title matches.
                    if (streamingData.Name.ToLower() == title.ToLower())
                    {
                        return streamingData;
                    }
                }
                catch
                {
                    continue;
                }

            }
            return null;
        }
    }
}
