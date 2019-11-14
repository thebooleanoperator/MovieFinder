using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Genres
    {
        public Genres()
        {
        }
        
        public Genres(ImdbInfoDto imdbInfo, Movies movie)
        {
            if (movie == null)
            {
                throw new ArgumentException($"Movie for genres cannot be null.");
            }

            if (imdbInfo == null)
            {
                throw new ArgumentException($"Movie for genres cannot be null.");
            }

            if (movie.MovieId <= 0)
            {
                throw new ArgumentException($"MovieId for genres cannot be null.");
            }

            if (imdbInfo.Genre == null)
            {
                throw new ArgumentException($"Genres cannot be null.");
            }

            var genres = imdbInfo.Genre.ToLower();

            MovieId = movie.MovieId;
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
