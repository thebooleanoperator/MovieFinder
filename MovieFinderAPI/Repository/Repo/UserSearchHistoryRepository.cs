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

        public IEnumerable<UserSearchHistory> GetAll(int userId)
        {
            return DbSet.Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated).Take(10);
        }
    }
}
