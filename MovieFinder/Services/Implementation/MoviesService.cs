using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Services.Interface;
using System;
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

        public async Task<List<RapidImdbDto>> GetImdbIdsByTitle(string title, int? year)
        {
            var request = RapidRequestSender.GetImdbIdsWithImdbAPI(title, year);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var rateLimit = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null; 

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response, true);

            if (parsedJson == null) { return null; }

            //Get each movie returned from search. 
            var searchResults = parsedJson["Search"].Children().ToList();
            var rapidDtos = new List<RapidImdbDto>();
            //Iterate through the search results and convert each Jmovie into a Movies object, 
            //then check if the title and year match. Save and break on true. 
            foreach (var Jmovie in searchResults)
            {
                try
                {
                    //Get the ImdbId by converting JObject to ImdbId.
                    RapidImdbDto rapidDto = Jmovie.ToObject<RapidImdbDto>();

                    var lowerMovieTitle = rapidDto.Title.ToLower();
                    var lowerTitle = title.ToLower();
                    // Only return objects that have similart titles and matching year (if provided).
                    if (lowerMovieTitle.Contains(lowerTitle) && (rapidDto.Year == year || year == null))
                    {
                        // If the rate limit fails to parse, throw general sytem exception. 
                        rapidDto.RequestsRemaining = int.TryParse(rateLimit, out var requestsRemaining) ? requestsRemaining : throw new Exception(); 
                        rapidDtos.Add(rapidDto);
                    }
                }
                catch
                {
                    continue;
                }

            }
            return rapidDtos;
        }

        public async Task<RapidImdbDto> GetImdbIdById(string imdbId)
        {
            if (imdbId == null)
            {
                return null;
            }

            var request = RapidRequestSender.GetImdbIdsWithImdbAPI(imdbId);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response, true);

            if (parsedJson == null) { return null; }

            //Get the ImdbInfoDto by converting JObject.
            try
            {
                return parsedJson.ToObject<RapidImdbDto>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<RapidImdbDto>> GetOnlyIdByTitle(string title)
        {
            var request = RapidRequestSender.GetImdbIdWithBackupImdbAPI(title);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response);

            if (parsedJson == null) { return null; }

            //Get each movie returned from search. 
            var searchResults = parsedJson["titles"].Children().ToList();
            var rapidDtos = new List<RapidImdbDto>();
            //Iterate through the search results and convert each Jmovie into a Movies object, 
            //then check if the title and year match. Save and break on true. 
            foreach (var Jmovie in searchResults)
            {
                try
                {
                    RapidImdbDto rapidDto = Jmovie.ToObject<RapidImdbDto>();
                    rapidDtos.Add(rapidDto);
                }
                catch
                {
                    continue;
                }
            }

            return rapidDtos;
        }

        public async Task<ImdbInfoDto> GetMovieInfo([FromBody] ImdbIds imdbId)
        {
            if (imdbId == null)
            {
                return null;
            }

            var request = RapidRequestSender.GetAllMovieInfoWithImdbAPI(imdbId.ImdbId, $"{imdbId.Year}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var jsonAndResponse = await HttpValidator.ValidateAndParseResponse(response, true);

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

        public async Task<StreamingDataDto> GetStreamingData(string title)
        {
            if (title == null)
            {
                return null;
            }

            var request = RapidRequestSender.UtellyRapidRequest(title);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            var parsedResponse = await HttpValidator.ValidateAndParseResponse(response);

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
