using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Collections.Generic;
using System.Linq;


namespace MovieFinder.Repository.Repo
{
    public class LikedMoviesRepository : MovieFinderRepository<LikedMovies>, ILikedMoviesRepository
    {
        private DbContext _context; 
        public LikedMoviesRepository(DbContext context) : base(context)
        {
            _context = context; 
        }

        public IEnumerable<LikedMovies> GetAllByUserId(int userId, int? page, int? count)
        {
            if (page == null || count == null)
            {
                return DbSet.Where(lm => lm.UserId == userId);
            }

            var orderedLikedMovies = DbSet.OrderByDescending(lm => lm.DateCreated);

            return orderedLikedMovies.Skip((int)page * (int)count).Take((int)count);
        }
    }
}
