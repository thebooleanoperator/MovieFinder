using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class SynopsisRepository : MovieFinderRepository<Synopsis>, ISynopsisRepository 
    {
        public SynopsisRepository(DbContext context) : base(context)
        {

        }

        public Synopsis GetByMovieId(int movieId)
        {
            //This will throw an error if none or more than one are found. 
            var synopsis = DbSet.Where(m => m.MovieId == movieId).Single();

            return synopsis;
        }
    }
}
