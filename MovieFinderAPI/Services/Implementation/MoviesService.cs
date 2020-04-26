using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Enum;
using MovieFinder.Models;
using MovieFinder.Repository;
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
        private UnitOfWork _unitOfWork; 

        public MoviesService(IHttpClientFactory clientFactory, IRateLimitsService rateLimitsService, MovieFinderContext movieFinderContext)
        {
            _clientFactory = clientFactory;
            _rateLimitsService = rateLimitsService;
            _unitOfWork = new UnitOfWork(movieFinderContext); 
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

            await _rateLimitsService.Update(RateLimitsEnum.ImdbAlternative, newRemainingRequests); 

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response, true);

            if (parsedJson == null) { return null; }

            var searchResults = parsedJson["Search"].Children().ToList();
            var rapidDtos = new List<RapidImdbDto>();

            foreach (var Jmovie in searchResults)
            {
                try
                {
                    RapidImdbDto rapidDto = Jmovie.ToObject<RapidImdbDto>();

                    var lowerMovieTitle = rapidDto.Title.ToLower();
                    var lowerTitle = title.ToLower();

                    if (lowerMovieTitle.Contains(lowerTitle) && (rapidDto.Year == year || year == null))
                    {
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

            // Don't hit the imdb alt API if there are no requests left.
            if (!_rateLimitsService.IsRequestsRemaining(RateLimitsEnum.ImdbAlternative))
            {
                return null;
            }

            var request = RapidRequestSender.GetImdbIdsWithImdbAPI(imdbId);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            // THIS IS A RATE LIMITED API. LIMIT TO 1000 REQUESTS PER DAY. 
            var requestsRemainingString = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null;
            var newRemainingRequests = int.TryParse(requestsRemainingString, out var rateLimit) ? rateLimit : throw new Exception();

            await _rateLimitsService.Update(RateLimitsEnum.ImdbAlternative, newRemainingRequests);

            var parsedJson = await HttpValidator.ValidateAndParseResponse(response, true);

            if (parsedJson == null) { return null; }

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

            var searchResults = parsedJson["titles"].Children().ToList();
            var rapidDtos = new List<RapidImdbDto>();

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
                var rapidMovieDto = new RapidMovieDto();
                rapidMovieDto.HasError = true;
                rapidMovieDto.ErrorMessage = "Imdb Id does not exist.";
                return rapidMovieDto;
            }

            // Don't hit the imdb alt API if there are no requests left.
            if (!_rateLimitsService.IsRequestsRemaining(RateLimitsEnum.ImdbAlternative))
            {
                var rapidMovieDto = new RapidMovieDto();
                rapidMovieDto.HasError = true;
                rapidMovieDto.ErrorMessage = "Max requests reached. Try again tomorrow.";
                return rapidMovieDto;
            }

            var request = RapidRequestSender.GetAllMovieInfoWithImdbAPI(imdbId.ImdbId, $"{imdbId.Year}");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            // THIS IS A RATE LIMITED API. LIMIT TO 1000 REQUESTS PER DAY. 
            var requestsRemainingString = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null;
            var newRemainingRequests = int.TryParse(requestsRemainingString, out var rateLimit) ? rateLimit : throw new Exception();

            await _rateLimitsService.Update(RateLimitsEnum.ImdbAlternative, newRemainingRequests);

            var jsonAndResponse = await HttpValidator.ValidateAndParseResponse(response, true);

            if (jsonAndResponse == null) { return null; }

            var parsedJson = jsonAndResponse;
            try
            {
                return parsedJson.ToObject<RapidMovieDto>();
            }
            catch
            {
                var rapidMovieDto = new RapidMovieDto();
                rapidMovieDto.HasError = true;
                rapidMovieDto.ErrorMessage = "Failed To Parse Movie Info."; 
                return rapidMovieDto;
            }
        }

        public async Task<RapidStreamingDto> GetStreamingData(string title, string imdbId)
        {
            if (title == null)
            {
                return null;
            }

            // Don't hit the imdb alt API if there are no requests left.
            if (!_rateLimitsService.IsRequestsRemaining(RateLimitsEnum.Utelly))
            {
                return null;
            }

            var request = RapidRequestSender.UtellyRapidRequest(title);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            // THIS IS A RATE LIMITED API. LIMIT TO 1000 REQUESTS PER DAY. 
            var requestsRemainingString = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null;
            var newRemainingRequests = int.TryParse(requestsRemainingString, out var rateLimit) ? rateLimit : throw new Exception();

            await _rateLimitsService.Update(RateLimitsEnum.Utelly, newRemainingRequests);

            var parsedResponse = await HttpValidator.ValidateAndParseResponse(response);

            if (parsedResponse == null) { return null; }

            var streamingResults = parsedResponse["results"].Children().ToList();

            foreach (var Jdata in streamingResults)
            {
                try
                {
                    var streamingData = Jdata.ToObject<RapidStreamingDto>();
                    //Only return the data if the streaming data response matches title and imdbId. 
                    if (StreamingDataIsMatch(streamingData, title, imdbId))
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

        public MoviesDto GetCompleteMovie(Movies movie)
        {
            // Get Streaming Data, Synopsis, and Genres to return all movie info.
            var streamingData = _unitOfWork.StreamingData.GetByMovieId(movie.MovieId);
            var synopsis = _unitOfWork.Synopsis.GetByMovieId(movie.MovieId);
            var genres = _unitOfWork.Genres.GetByMovieId(movie.MovieId);

            return  new MoviesDto(movie, genres, streamingData, synopsis);
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
