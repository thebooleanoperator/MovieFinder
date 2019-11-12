using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository
{
    public class MoviesRepository : MovieFinderRepository<Movies>, IMoviesRepository
    {
        private DbContext _context;

        public MoviesRepository(DbContext context) : base(context)
        {

        }

        public Movies GetByImdbId(string imdbId)
        {
            return DbSet.Where(m => m.ImdbId == imdbId).SingleOrDefault();
        }
    }
}
