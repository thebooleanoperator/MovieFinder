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

        public IEnumerable<Movies> Get(List<int> movieIds)
        {
            var movies = new List<Movies>(); 
            foreach (var movieId in movieIds)
            {
                var movie = DbSet.Where(x => x.MovieId == movieId).First();
                movies.Add(movie);
            }

            return movies; 
        }
    }
}
