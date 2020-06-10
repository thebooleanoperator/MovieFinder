using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface IUserSearchHistoryRepository : IMovieFinderRepository<UserSearchHistory>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<UserSearchHistory> GetAll(int userId);
    }
}
