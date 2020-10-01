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
        
        public IEnumerable<UserSearchHistory> GetAllByUserId(int userId, int? skip = null, int? historyLength = null)
        {
            if (skip == null || historyLength == null)
            {
                return DbSet.Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated);
            }

            return DbSet.Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated)
                .Take((int)historyLength).Skip((int)skip);
        }
    }
}
