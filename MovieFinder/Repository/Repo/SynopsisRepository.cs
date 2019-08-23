using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class SynopsisRepository : MovieFinderRepository<Synopsis>, ISynopsisRepository 
    {
        private DbContext _context; 

        public SynopsisRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public Synopsis GetByMovieId(int movieId)
        {
            var synopsis = DbSet.Where(m => m.MovieId == movieId).Single();

            return synopsis;
        }
    }
}
