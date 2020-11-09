using MovieFinder.DtoModels;
using MovieFinder.Repository.BaseEntity;
using System;

namespace MovieFinder.Models
{
    public partial class UserSearchHistory : BaseEntity
    {
        public UserSearchHistory()
        {

        }

        public UserSearchHistory(UserSearchHistoryDto userSearchHistoryDto, int userId)
        {
            if (userSearchHistoryDto == null)
            {
                throw new ArgumentException($"{nameof(userSearchHistoryDto)} is required.");
            }

            if (userSearchHistoryDto.MovieId <= 0)
            {
                throw new ArgumentException($"{nameof(userSearchHistoryDto.MovieId)} is required.");
            }

            if (String.IsNullOrEmpty(userSearchHistoryDto.Title))
            {
                throw new ArgumentException($"{nameof(userSearchHistoryDto.Title)} is required.");
            }

            if (String.IsNullOrEmpty(userSearchHistoryDto.Poster))
            {
                throw new ArgumentException($"{nameof(userSearchHistoryDto.Poster)} is required.");
            }

            MovieId = userSearchHistoryDto.MovieId;
            Title = userSearchHistoryDto.Title;
            Poster = userSearchHistoryDto.Poster;
            UserId = userId;
        }
    }
}
