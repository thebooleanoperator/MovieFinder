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

        public UserSearchHistory(UserSearchHistoryDto userSearchHistoryDto)
        {
            if (userSearchHistoryDto == null)
            {
                throw new ArgumentException($"{nameof(userSearchHistoryDto)} is required.");
            }

            if (userSearchHistoryDto.MovieId <= 0)
            {
                throw new ArgumentException($"{nameof(userSearchHistoryDto.MovieId)} is required.");
            }

            if (userSearchHistoryDto.UserId <= 0)
            {
                throw new ArgumentException($"{nameof(userSearchHistoryDto.UserId)} must be greater than 0.");
            }

            MovieId = userSearchHistoryDto.MovieId;
            UserId = userSearchHistoryDto.UserId;
        }
    }
}
