using MovieFinder.Models;
using System;

namespace MovieFinder.DtoModels
{
    public class MovieTitlesDto
    {
        public string MovieTitle { get; set; }
        public int Year { get; set; }

        public MovieTitlesDto()
        {
        }

        public MovieTitlesDto(Movies movie)
        {
            if (movie == null)
            {
                throw new ArgumentException("movie object cannot be null"); 
            }

            MovieTitle = movie.Title;
            Year = movie.Year;
        }
    }
}
