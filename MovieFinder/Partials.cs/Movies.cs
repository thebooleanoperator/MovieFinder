using MovieFinder.Partialscs;
using System;

namespace MovieFinder.Models
{
    public partial class Movies
    {
        public Movies(MoviesDto moviesDto)
        {
            if (moviesDto.Genre.Length <= 0)
            {
                throw new ArgumentException($"{moviesDto.Genre} must be have characters");
            }

            if (moviesDto.Year <= 0)
            {
                throw new ArgumentException($"{moviesDto.Year} must be greater than 0");
            }

            if (moviesDto.Director.Length <= 0)
            {
                throw new ArgumentException($"{moviesDto.Director} must be have characters");
            }

            if (moviesDto.Title.Length <=0)
            {
                throw new ArgumentException($"{moviesDto.Title} must be have characters");
            }

            if (moviesDto.RunTime <= 0 )
            {
                throw new ArgumentException($"{moviesDto.RunTime} must be greater than 0");
            }

            Genre = moviesDto.Genre;
            Year = moviesDto.Year;
            Director = moviesDto.Director;
            Title = moviesDto.Title;
            RunTime = moviesDto.RunTime;
        }
    }
}
