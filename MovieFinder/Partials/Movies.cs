using MovieFinder.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieFinder.Models
{
    public partial class Movies
    {
        public Movies()
        {
        }

        public Movies(ImdbInfoDto imdbInfo, ImdbIds imdbId)
        {
            if(imdbInfo == null)
            {
                throw new ArgumentException($"{imdbInfo} must not be null");
            }

            if (imdbInfo.Director == null)
            {
                throw new ArgumentException($"{imdbInfo.Director} must be have characters");
            }

            if (imdbInfo.Title == null)
            {
                throw new ArgumentException($"{imdbInfo.Title} must be have characters");
            }

            if (imdbInfo.RunTime == null)
            {
                throw new ArgumentException($"{imdbInfo.RunTime} must be have characters");
            }

            if (imdbInfo.Ratings == null)
            {
                throw new ArgumentException($"{imdbInfo.Ratings} cannot be null");
            }

            if (imdbInfo.ImdbId == null)
            {
                throw new ArgumentException($"{imdbInfo.ImdbId} must be have characters");
            }

            if (imdbInfo.Poster == null)
            {
                throw new ArgumentException($"{imdbInfo.Poster} must be have characters");

            }

            if (imdbId.Year < 0)
            {
                throw new ArgumentException($"{imdbId.Year} must be have characters");
            }

            Director = imdbInfo.Director;
            Title = imdbInfo.Title;
            RunTime = getMovieRunTime(imdbInfo.RunTime);
            ImdbId = imdbInfo.ImdbId;
            ImdbRating = getMovieRating("Internet Movie Database", imdbInfo.Ratings);
            RottenTomatoesRating = getMovieRating("Rotten Tomatoes", imdbInfo.Ratings);
            Year = imdbId.Year;
            Poster = imdbInfo.Poster;
        }

        private int? getMovieRunTime(string runTimeString)
        {
            var splitRunTime = runTimeString.Split(' ');
            int runTime; 
            if(int.TryParse(splitRunTime[0], out runTime))
            {
                return runTime; 
            }
            else
            {
                return null;
            }
    
        }

        private int? getMovieRating(string source, List<RatingsDto> ratingsList)
        {
            var rating = ratingsList.Where(r => r.Source == source).Select(r => r.Value).SingleOrDefault();

            if (source == "Internet Movie Database")
            {
                return convertImdbRating(rating);
            }

            if (source == "Rotten Tomatoes")
            {
                return convertRottenRating(rating);
            }

            return null;
        }

        private int? convertImdbRating(string rating)
        {
            int imdbRating;
            if (int.TryParse(rating, out imdbRating))
            {
                return imdbRating; 
            }
            else
            {
                return null;
            }
        }

        private int? convertRottenRating(string rating)
        {
            int rottenRating;
            if (int.TryParse(rating, out rottenRating))
            {
                return rottenRating;
            }
            else
            {
                return null;
            }
        }
    }
}
