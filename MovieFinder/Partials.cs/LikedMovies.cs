using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class LikedMovies
    {
        public LikedMovies(LikedMoviesDto likedMoviesDto)
        {
            if(likedMoviesDto.MovieId <= 0)
            {
                throw new ArgumentException($"MovieId {likedMoviesDto.MovieId} must be greater than 0.");
            }

            if (likedMoviesDto.UserId <= 0)
            {
                throw new ArgumentException($"MovieId {likedMoviesDto.MovieId} must be greater than 0.");
            }

            MovieId = likedMoviesDto.MovieId;
            UserId = likedMoviesDto.UserId; 

        }
    }
}
