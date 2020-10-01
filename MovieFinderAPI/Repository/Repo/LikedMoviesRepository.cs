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

        public IEnumerable<LikedMovies> GetAllByUserId(int userId, int? skip = null, int? count = null)
        {
            if (skip == null || count == null)
            {
                return DbSet.Where(lm => lm.UserId == userId);
            }

            return DbSet.Where(lm => lm.UserId == userId)
                .OrderByDescending(lm => lm.DateCreated).Skip((int)skip).Take((int)count);
        }
    }
}
