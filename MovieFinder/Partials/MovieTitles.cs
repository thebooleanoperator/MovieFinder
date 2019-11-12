using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class MovieTitles
    {
        public MovieTitles()
        {
        }

        public MovieTitles(MovieTitlesDto movieTitlesDto)
        {
            if (movieTitlesDto.MovieTitle.Length == 0 || movieTitlesDto.MovieTitle == null)
            {
                throw new ArgumentException("movie title cannot be null.");
            }

            if (movieTitlesDto.Year < 0)
            {
                throw new ArgumentException("movie year must be greater than 0.");
            }

            MovieTitle = movieTitlesDto.MovieTitle;
            Year = movieTitlesDto.Year;
        }
    }
}
