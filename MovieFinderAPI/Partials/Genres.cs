using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Genres
    {
        public Genres()
        {
        }
        
        public Genres(RapidMovieDto rapidMovieDto)
        {
            if (rapidMovieDto == null)
            {
                throw new ArgumentException($"Movie for genres cannot be null.");
            }

            if (rapidMovieDto.Genre == null)
            {
                throw new ArgumentException($"Genres cannot be null.");
            }

            var genres = rapidMovieDto.Genre.ToLower();

            Action = genres.Contains("action");
            Adventure = genres.Contains("adventure");
            Horror = genres.Contains("horror");
            Biography = genres.Contains("biography");
            Comedy = genres.Contains("comedy");
            Crime = genres.Contains("crime");
            Thriller = genres.Contains("thriller");
            Romance = genres.Contains("romance");
        }
    }
}
