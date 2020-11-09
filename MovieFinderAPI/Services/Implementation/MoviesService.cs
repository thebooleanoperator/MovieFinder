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
        private IStreamingDataService _streamingDataService; 
        private UnitOfWork _unitOfWork; 

        public MoviesService(
            IHttpClientFactory clientFactory, 
            IRateLimitsService rateLimitsService, 
            IStreamingDataService streamingDataService, 
            MovieFinderContext movieFinderContext)
        {
            _clientFactory = clientFactory;
            _rateLimitsService = rateLimitsService;
            _streamingDataService = streamingDataService;
            _unitOfWork = new UnitOfWork(movieFinderContext); 
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
     }
}
