using MovieFinder.Models;
using System;
using System.Collections.Generic;

namespace MovieFinder.DtoModels
{
    public class RapidImdbDto
    {
        public string ImdbId { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
        public int Year { get; set; }

        public RapidImdbDto()
        {

        }

        public RapidImdbDto(ImdbIds imdbId)
        {
            if (imdbId == null)
            {
                throw new ArgumentException($"nameof{imdbId} is required."); 
            }

            ImdbId = imdbId.ImdbId;
            Title = imdbId.Title;
            Year = imdbId.Year; 
        }

        /// <summary>
        /// Converts an IEnumerable of ImdbIds to an IEnumerable of RapidImdbDtos
        /// </summary>
        /// <param name="imdbIds"></param>
        /// <returns></returns>
        public static IEnumerable<RapidImdbDto> ConvertFromImdbIds(IEnumerable<ImdbIds> imdbIds)
        {
            var rapidDtos = new List<RapidImdbDto>();

            foreach(var imdbId in imdbIds)
            {
                var rapidDto = new RapidImdbDto(imdbId);
                rapidDtos.Add(rapidDto); 
            }

            return rapidDtos; 
        }
    }
}
