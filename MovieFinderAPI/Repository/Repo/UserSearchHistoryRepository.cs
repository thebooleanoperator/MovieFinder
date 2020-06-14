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
        // ToDo: Improve
        public IEnumerable<int> GetMovieIdsByUserId(int userId, int? historyLength = null)
        {
            var allSearchedMovieIds = DbSet.Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated).Select(x => x.MovieId);
            if (historyLength == null)
            {
                return allSearchedMovieIds;
            }

            
            var searchedMovieIds = new List<int>();
            var resultsAdded = 0; 
            foreach(var movieId in allSearchedMovieIds)
            {
                if (!searchedMovieIds.Contains(movieId))
                {
                    searchedMovieIds.Add(movieId);
                    resultsAdded += 1;
                }
                if (resultsAdded == historyLength)
                {
                    break;
                }
            }

            return searchedMovieIds;
        }
    }
}
