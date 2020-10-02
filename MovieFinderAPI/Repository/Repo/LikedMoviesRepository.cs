using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Collections.Generic;
using System.Linq;


namespace MovieFinder.Repository.Repo
{
    public class LikedMoviesRepository : MovieFinderRepository<LikedMovies>, ILikedMoviesRepository
    {
        public LikedMoviesRepository(DbContext context) : base(context)
        {
            
        }

        public LikedMovies GetByMovieId(int movieId, int userId)
        {
            return DbSet.Where(lm => lm.MovieId == movieId && lm.UserId == userId).SingleOrDefault();
        }

        public IEnumerable<LikedMovies> GetAll(int userId, int? skip = null, int? count = null)
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
