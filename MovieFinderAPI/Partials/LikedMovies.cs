using MovieFinder.DtoModels;
using MovieFinder.Repository.BaseEntity;
using System;

namespace MovieFinder.Models
{
    public partial class LikedMovies : BaseEntity
    {
        public LikedMovies()
        {

        }

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

            if (String.IsNullOrEmpty(likedMoviesDto.Title))
            {
                throw new ArgumentException($"MovieId {likedMoviesDto.MovieId} must be greater than 0.");
            }

            if (String.IsNullOrEmpty(likedMoviesDto.Poster))
            {
                throw new ArgumentException($"MovieId {likedMoviesDto.MovieId} must be greater than 0.");
            }

            MovieId = likedMoviesDto.MovieId;
            UserId = likedMoviesDto.UserId;
            Title = likedMoviesDto.Title;
            Poster = likedMoviesDto.Poster;
        }
    }
}
