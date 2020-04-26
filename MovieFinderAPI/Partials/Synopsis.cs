using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Synopsis
    {
        public Synopsis()
        {
        }

        public Synopsis(RapidMovieDto rapidMovieData, Movies movie)
        {
            if (rapidMovieData.Plot == null)
            {
                throw new ArgumentException($"{rapidMovieData.Plot} must have characters");
            }

            if (movie.MovieId <= 0)
            {
                throw new ArgumentException($"{movie.MovieId} must be greater than 0");
            }

            Plot = rapidMovieData.Plot;
            MovieId = movie.MovieId;
        }
    }
}
