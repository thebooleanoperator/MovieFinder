﻿using MovieFinder.DtoModels;
using MovieFinder.Enum;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Services.Implementation
{
    public class StreamingDataService : IStreamingDataService 
    {
        private IHttpClientFactory _clientFactory;
        private UnitOfWork _unitOfWork;
        private IRateLimitsService _rateLimitsService; 

        public StreamingDataService(IHttpClientFactory clientFactory, MovieFinderContext movieFinderContext, IRateLimitsService rateLimitsService)
        {
            _clientFactory = clientFactory;
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _rateLimitsService = rateLimitsService;
        }

        public async Task<RapidStreamingDto> GetStreamingData(string imdbId)
        {
            // Don't hit the imdb alt API if there are no requests left.
            if (!_rateLimitsService.IsRequestsRemaining(RateLimitsEnum.Utelly))
            {
                return null;
            }

            var request = RapidRequestSender.UtellyRapidRequest(imdbId);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            // THIS IS A RATE LIMITED API. LIMIT TO 1000 REQUESTS PER DAY. 
            var requestsRemainingString = response.Headers.TryGetValues("x-ratelimit-requests-remaining", out var values) ? values.FirstOrDefault() : null;
            var newRemainingRequests = int.TryParse(requestsRemainingString, out var rateLimit) ? rateLimit : throw new Exception();

            await _rateLimitsService.Update(RateLimitsEnum.Utelly, newRemainingRequests);

            var parsedResponse = await HttpValidator.ValidateAndParseStreamingDataResponse(response);

            if (parsedResponse == null) { return null; }

            try
            {
                return parsedResponse.ToObject<RapidStreamingDto>();
            }
            catch
            {
                return null; 
            }
        }

        public async Task UpdateStreamingData(MoviesDto moviesDto)
        {
            var lastUpdated = moviesDto.StreamingData.LastUpdated;
            var needsUpdate = DateTime.Now.Subtract(lastUpdated).Days <= 7 ? false : true;
            // Only update if the there has been no update in last 7 days.
            if (needsUpdate == true)
            {
                var updatedStreamingData = await GetStreamingData(moviesDto.ImdbId);

                moviesDto.StreamingData.Patch(updatedStreamingData);

                _unitOfWork.StreamingData.Update(moviesDto.StreamingData);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
