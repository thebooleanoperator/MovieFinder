using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class MovieTitles
    {
        public MovieTitles()
        {
        }

        public MovieTitles(string title)
        {
            if (title.Length == 0 || title == null)
            {
                throw new ArgumentException("movie title cannot be null.");
            }

            MovieTitle = title;
        } 
    }
}
