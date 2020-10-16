using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class UserSearchHistoryRepository : MovieFinderRepository<UserSearchHistory>, IUserSearchHistoryRepository
    {
        public UserSearchHistoryRepository(DbContext context): base(context)
        {

        } 
        
        public IEnumerable<UserSearchHistory> GetAllByUserId(int userId, int? skip = null, int? count = null)
        {
            var searchHistory = DbSet.Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated);
            // Return all search history if not using pagination.
            if (skip == null || count == null)
            {
                return searchHistory;
            }

            return searchHistory.Skip((int)skip).Take((int)count);
        }
    }
}
