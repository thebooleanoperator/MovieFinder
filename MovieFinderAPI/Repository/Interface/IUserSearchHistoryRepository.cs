﻿using MovieFinder.DtoModels;
using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface IUserSearchHistoryRepository : IMovieFinderRepository<UserSearchHistory>
    {
        /// <summary>
        /// Gets the ten most recent userSearchHistorys using userId to sort by user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserSearchHistory> GetAllByUserId(int userId, int? historyLength = null);
    }
}
