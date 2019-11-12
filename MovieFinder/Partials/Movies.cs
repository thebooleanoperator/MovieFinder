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

            if (imdbInfo.Genre == null)
            {
                throw new ArgumentException($"{imdbInfo.Genre} must be have characters");
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

            if (imdbInfo.Ratings.Count() < 3)
            {
                throw new ArgumentException($"{imdbInfo.RunTime} must be have characters");
            }

            if (imdbInfo.ImdbId == null)
            {
                throw new ArgumentException($"{imdbInfo.ImdbId} must be have characters");
            }

            if (imdbId.Year < 0)
            {
                throw new ArgumentException($"{imdbId.Year} must be have characters");
            }

            Genre = imdbInfo.Genre;
            Director = imdbInfo.Director;
            Title = imdbInfo.Title;
            RunTime = imdbInfo.RunTime;
            ImdbId = imdbInfo.ImdbId;
            ImdbRating = getImdbRating(imdbInfo.Ratings);
            RottenTomatoesRating = getRottenRating(imdbInfo.Ratings);
            Year = imdbId.Year;
        }

        private string getImdbRating(List<RatingsDto> ratingsList)
        {
            var imdbRatingsObject = ratingsList[0];
            return imdbRatingsObject.Value;
        }

        private string getRottenRating(List<RatingsDto> ratingsList)
        {
            var rottenRatingObject = ratingsList[1];
            return rottenRatingObject.Value;
        }
    }
}
