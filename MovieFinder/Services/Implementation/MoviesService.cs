using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Enum;
using MovieFinder.Models;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Services.Implementation
{
    public class MoviesService : IMoviesService
    {
        private IHttpClientFactory _clientFactory;
        private IRateLimitsService _rateLimitsService; 

        public MoviesService(IHttpClientFactory clientFactory, IRateLimitsService rateLimitsService)
        {
            _clientFactory = clientFactory;
            _rateLimitsService = rateLimitsService; 
        }

        public async Task<List<RapidImdbDto>> GetImdbIdsByTitle(string title, int? year)
        {
            // Don't hit the imdb alt API if there are no requests left.
            if (!_rateLimitsService.IsRequestsRemaining(RateLimitsEnum.ImdbAlternative))
            {
                return null;
            }

            var request = RapidRequestSender.GetImdbIdsWithImdbAPI(title, year);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            // THIS IS A RATE LIMITED API. LIMIT TO 1000 REQUESTS PER DAY. 
            var requestsRemainingString = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null;
            var newRemainingRequests = int.TryParse(requestsRemainingString, out var reamining) ? reamining : throw new Exception();



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

            // THIS IS A RATE LIMITED API. LIMIT TO 1000 REQUESTS PER DAY. 
            var requestsRemainingString = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null;
            var requestsRemaining = int.TryParse(requestsRemainingString, out var rateLimit) ? rateLimit : throw new Exception();

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response, true);

            if (parsedJson == null) { return null; }

            //Get the ImdbInfoDto by converting JObject.
            try
            {
                var rapidDto =  parsedJson.ToObject<RapidImdbDto>();
                rapidDto.RequestsRemaining = requestsRemaining; 
                return rapidDto;
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

        public async Task<RapidMovieDto> GetMovieInfo([FromBody] ImdbIds imdbId)
        {
            if (imdbId == null)
            {
                return null;
            }

            var request = RapidRequestSender.GetAllMovieInfoWithImdbAPI(imdbId.ImdbId, $"{imdbId.Year}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            // THIS IS A RATE LIMITED API. LIMIT TO 1000 REQUESTS PER DAY. 
            var requestsRemainingString = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null;
            var requestsRemaining = int.TryParse(requestsRemainingString, out var rateLimit) ? rateLimit : throw new Exception();

            var jsonAndResponse = await HttpValidator.ValidateAndParseResponse(response, true);

            if (jsonAndResponse == null) { return null; }

            var parsedJson = jsonAndResponse;
            // If parse fails, return null.
            try
            {
                //Get the ImdbInfoDto by converting JObject.
                var rapidMovieDto = parsedJson.ToObject<RapidMovieDto>();
                rapidMovieDto.RequestsRemaining = requestsRemaining;
                return rapidMovieDto; 
            }
            catch
            {
                return null;
            }
        }

        public async Task<RapidStreamingDto> GetStreamingData(string title, string imdbId)
        {
            if (title == null)
            {
                return null;
            }

            var request = RapidRequestSender.UtellyRapidRequest(title);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            // THIS IS A RATE LIMITED API. LIMIT TO 1000 REQUESTS PER DAY. 
            var requestsRemainingString = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null;
            var requestsRemaining = int.TryParse(requestsRemainingString, out var rateLimit) ? rateLimit : throw new Exception();

            var parsedResponse = await HttpValidator.ValidateAndParseResponse(response);

            if (parsedResponse == null) { return null; }

            //Get the ImdbInfoDto by converting JObject.
            var streamingResults = parsedResponse["results"].Children().ToList();

            foreach (var Jdata in streamingResults)
            {
                try
                {
                    var streamingData = Jdata.ToObject<RapidStreamingDto>();
                    //Only return the data if the streaming data response matches title and imdbId. 
                    if (StreamingDataIsMatch(streamingData, title, imdbId))
                    {
                        streamingData.RequestsRemaining = requestsRemaining; 
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

        /// <summary>
        /// Private helper function to match rapid streaming data response with user selected movie. 
        /// </summary>
        /// <param name="rapidStreamingData"></param>
        /// <param name="movieTitle"></param>
        /// <param name="imdbId"></param>
        /// <returns></returns>
        private bool StreamingDataIsMatch(RapidStreamingDto rapidStreamingData, string movieTitle, string imdbId)
        {
            var rapidTitle = rapidStreamingData.Name.ToLower(); 
            if (rapidTitle != movieTitle.ToLower())
            {
                return false; 
            }

            var rapidImdbId = rapidStreamingData.External_Ids.Imdb.Id; 
            if (rapidImdbId != imdbId)
            {
                return false;
            }

            // If both checks pass, we have a match. 
            return true; 
        }
    }
}
