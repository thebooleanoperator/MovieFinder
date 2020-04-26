using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class ImdbIds
    {
        public ImdbIds()
        {

        }

        public ImdbIds(RapidImdbDto rapidDto)
        {
            if (rapidDto == null)
            {
                throw new ArgumentException($"{nameof(rapidDto)} is required."); 
            }

            if (String.IsNullOrEmpty(rapidDto.ImdbId))
            {
                throw new ArgumentException($"{nameof(rapidDto.ImdbId)} is required.");
            }

            if (String.IsNullOrEmpty(rapidDto.Title))
            {
                throw new ArgumentException($"{nameof(rapidDto.Title)} is required.");
            }

            if (rapidDto.Year <= 0)
            {
                throw new ArgumentException($"{nameof(rapidDto.Year)} must be greater than zero.");
            }

            ImdbId = rapidDto.ImdbId;
            Title = rapidDto.Title;
            Year = rapidDto.Year; 
        }
    }
}
