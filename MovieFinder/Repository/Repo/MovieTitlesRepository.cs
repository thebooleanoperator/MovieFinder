using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Collections.Generic;
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

        public IEnumerable<MovieTitles> GetNext(int page, int count)
        {
            return DbSet.Skip((page - 1) * count).Take(count);
        }
    }
}
