using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace MovieFinder.Repository
{
    public class MoviesRepository : MovieFinderRepository<Movies>, IMoviesRepository
    {
        public MoviesRepository(DbContext context) : base(context)
        {
            
        }

        new public Movies Get(int movieId)
        {
            return DbSet.Where(x => x.MovieId == movieId)
                    .Include(x => x.Genre)
                    .Include(x => x.StreamingData).SingleOrDefault();
        }

        public IEnumerable<Movies> Get(IEnumerable<int> movieIds)
        {
            return DbSet.Where(x => movieIds.Contains(x.MovieId));
        }

        public Movies GetByImdbId(string imdbId)
        {
            return DbSet.Where(m => m.ImdbId == imdbId).SingleOrDefault();
        }

        public IEnumerable<Movies> GetAllByTitle(string title)
        {
            title = title.ToLower().Replace(" ", "");
            return DbSet.Where(m => m.Title.ToLower().Replace(" ", "").Contains(title));
        }

        public IEnumerable<Movies> GetAllRecommended()
        {
            return DbSet.Where(m => m.IsRec == true);
              
        }
    }
}
