using MovieFinder.Models;
using System;
using System.Collections.Generic;

namespace MovieFinder.DtoModels
{
    public class MoviesDto
    {
        public int MovieId { get; set; }
        public string Genre { get; set; }
        public int Year {get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public string RunTime { get; set; }
        public decimal? ImdbRating { get; set; }
        public decimal? RottenTomatoesRating { get; set; }
        public string ImdbId { get; set; }
        public int? NetflixId { get; set; }
        public bool IsRec { get; set; }
        public string Poster { get; set; }
        public Genres Genres { get; set; }

        public MoviesDto(Movies movie, Genres genres)
        {
            if (movie == null)
            {
                throw new ArgumentException($"{nameof(movie)} cannot be empty.");
            }

            MovieId = movie.MovieId;
            Year = movie.Year;
            Director = movie.Director;
            Title = movie.Title;
            RunTime = movie.RunTime.ToString();
            ImdbRating = movie.ImdbRating;
            RottenTomatoesRating = movie.RottenTomatoesRating;
            NetflixId = movie.NetflixId;
            IsRec = movie.IsRec;
            Genres = genres;
            Poster = movie.Poster;
        }
    }
}
