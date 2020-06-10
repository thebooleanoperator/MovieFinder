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

        public IEnumerable<Movies> GetMoviesFromFavorites(IEnumerable<LikedMovies> likedMovies)
        {
            var likedMovieIds = likedMovies.Select(lm => lm.MovieId).ToList();

            var movies = DbSet.Where(m => likedMovieIds.Contains(m.MovieId));

            var orderedMovies = movies.OrderBy(m => likedMovieIds.IndexOf(m.MovieId));

            return orderedMovies;
        }
    }
}
