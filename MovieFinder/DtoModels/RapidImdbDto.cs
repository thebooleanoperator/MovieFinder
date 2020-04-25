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

        /// <summary>
        /// Concats two lists of RapidImdbDtos together without concating any duplicates. 
        /// </summary>
        /// <param name="rapidDtos1"></param>
        /// <param name="rapidDtos2"></param>
        /// <returns></returns>
        public static IEnumerable<RapidImdbDto> CombineWithNoDuplicates(List<RapidImdbDto> rapidDtos1, List<RapidImdbDto> rapidDtos2)
        {
            var seenIds = new List<string>(); 

            foreach (var imdbId in rapidDtos1)
            {
                seenIds.Add(imdbId.ImdbId); 
            }

            foreach (var imdbId in rapidDtos2)
            {
                if (!seenIds.Contains(imdbId.ImdbId))
                {
                    rapidDtos1.Add(imdbId);
                    seenIds.Add(imdbId.ImdbId); 
                }
            }

            return rapidDtos1;
        }
    }
}
