using MovieFinder.DtoModels;
using MovieFinder.Enum;
using MovieFinder.Models;
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

            if (parsedResponse == null || !parsedResponse.HasValues)
            {
                return null;
            }

            try
            {
                return parsedResponse.ToObject<RapidStreamingDto>();
            }
            catch
            {
                throw new Exception();
            }
        }

        public async Task<StreamingData> GetUpdatedStreamingData(StreamingData streamingData, string imdbId)
        {
            var lastUpdated = streamingData.LastUpdated;
            var needsUpdate = DateTime.Now.Subtract(lastUpdated).Days <= 7 ? false : true;
            // Only update if the there has been no update in last 7 days.
            if (needsUpdate)
            {
                var updatedRapidStreamingDataDto = await GetStreamingData(imdbId);

                streamingData.Patch(updatedRapidStreamingDataDto);

                _unitOfWork.StreamingData.Update(streamingData);
                _unitOfWork.SaveChanges();
            }

            return streamingData;
        }
    }
}
