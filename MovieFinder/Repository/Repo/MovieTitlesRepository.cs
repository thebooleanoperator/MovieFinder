using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;

namespace MovieFinder.Repository.Repo
{
    public class MovieTitlesRepository : MovieFinderRepository<MovieTitles>, IMovieTitlesRepository
    {
        public MovieTitlesRepository(DbContext context) : base(context)
        {
        }
    }
}
