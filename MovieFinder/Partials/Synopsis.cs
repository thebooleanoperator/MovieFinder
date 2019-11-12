using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Synopsis
    {
        public Synopsis()
        {
        }

        public Synopsis(ImdbInfoDto imdbInfo, Movies movie)
        {
            if (imdbInfo.Plot == null)
            {
                throw new ArgumentException($"{imdbInfo.Plot} must have characters");
            }

            if (movie.MovieId <= 0)
            {
                throw new ArgumentException($"{movie.MovieId} must be greater than 0");
            }

            Plot = imdbInfo.Plot;
            MovieId = movie.MovieId;
        }
    }
}
