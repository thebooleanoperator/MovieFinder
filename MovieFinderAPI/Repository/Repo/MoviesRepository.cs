using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using MovieFinder.Repository.Repo;
using System.Collections.Generic;
using System.Linq;

namespace MovieFinder.Repository
{
    public class MoviesRepository : MovieFinderRepository<Movies>, IMoviesRepository
    {
        private DbContext _context; 
        public MoviesRepository(DbContext context) : base(context)
        {
            _context = context;
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
            return DbSet.Where(m => m.IsRec == true)
                .Include(x => x.StreamingData)
                .Include(x => x.Genre);
        }

        public IEnumerable<Movies> Get(IEnumerable<int> movieIds)
        {
            return DbSet.Where(x => movieIds.Contains(x.MovieId));
        }
    }
}
