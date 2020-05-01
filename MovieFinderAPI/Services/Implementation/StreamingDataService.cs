using MovieFinder.DtoModels;
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

        public StreamingDataService(IHttpClientFactory clientFactory,
            MovieFinderContext movieFinderContext, IMoviesService moviesService, IRateLimitsService rateLimitsService)
        {
            _clientFactory = clientFactory;
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _rateLimitsService = rateLimitsService;
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

        public async Task UpdateStreamingData(MoviesDto moviesDto)
        {
            var lastUpdated = moviesDto.StreamingData.LastUpdated;
            var needsUpdate = DateTime.Now.Subtract(lastUpdated).Days <= 7 ? false : true;
            // Only update if the there has been no update in last 7 days.
            if (needsUpdate == true)
            {
                var updatedStreamingData = await GetStreamingData(moviesDto.Title, moviesDto.ImdbId);

                moviesDto.StreamingData.Patch(updatedStreamingData);

                _unitOfWork.StreamingData.Update(moviesDto.StreamingData);
                _unitOfWork.SaveChanges();
            }
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
