using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class MovieTitlesRepository : MovieFinderRepository<MovieTitles>, IMovieTitlesRepository
    {
        public MovieTitlesRepository(DbContext context) : base(context)
        { 
        }

        public bool MovieTitleExists(string title, int year)
        {
            return DbSet.Any(m => m.MovieTitle == title && m.Year == year);
        }
    }
}
