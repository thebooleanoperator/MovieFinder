using MovieFinder.DtoModels;
using MovieFinder.Enum;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Services.Implementation
{
    public class ImdbIdsService : IImdbIdsService
    {
        private IHttpClientFactory _clientFactory; 
        private IRateLimitsService _rateLimitsService;
        
        public ImdbIdsService(IHttpClientFactory clientFactory, IRateLimitsService rateLimitsService)
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
    }
}
