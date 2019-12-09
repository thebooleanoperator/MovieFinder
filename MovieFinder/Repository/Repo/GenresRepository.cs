using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class GenresRepository : MovieFinderRepository<Genres>, IGenresRepository
    {
        public GenresRepository(DbContext context) : base(context)
        {

        }

        public Genres GetByMovieId(int movieId)
        {
            return DbSet.Where(m => m.MovieId == movieId).Single();
        }
    }
}
