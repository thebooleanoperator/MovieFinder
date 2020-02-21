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
                        _unitOfWork.ImdbIds.Add(imdbId);
                    }
                }
            }
            if (imdbIds == null || imdbIds.Count() == 0)
            {
                return null;
            }

            _unitOfWork.SaveChanges();
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

        /// <summary>
        /// Returns the netflix Id for a movie or null if the movie is not on netflix. 
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
                var streamingData = Jdata.ToObject<StreamingDataDto>();
                //Only return the data if title matches.
                if (streamingData.Name.ToLower() == title.ToLower())
                {
                    return streamingData;
                }
            }
            return null;
        }

        public void FillAssociatedTables(ImdbInfoDto imdbInfo, Movies movie, StreamingDataDto streamingDataDto, bool saveTables = true)
        {
            var streamingData = new StreamingData(streamingDataDto, movie);
            _unitOfWork.StreamingData.Add(streamingData);

            var synopsis = new Synopsis(imdbInfo, movie);
            _unitOfWork.Synopsis.Add(synopsis);

            var genres = new Genres(imdbInfo, movie);
            _unitOfWork.Genres.Add(genres);

            if (saveTables)
            {
                _unitOfWork.SaveChanges();
            }
        }
    }
}
