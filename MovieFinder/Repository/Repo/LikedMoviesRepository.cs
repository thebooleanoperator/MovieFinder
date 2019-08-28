using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Collections.Generic;
using System.Linq;


namespace MovieFinder.Repository.Repo
{
    public class LikedMoviesRepository : MovieFinderRepository<LikedMovies>, ILikedMoviesRepository
    {
        private DbContext _context; 
        public LikedMoviesRepository(DbContext context) : base(context)
        {
            _context = context; 
        }

        public List<Movies> GetAll(int userId)
        {
            var movies = _context.Set<Movies>();
            var likedMovies = DbSet
                                .Join(movies, lm => lm.MovieId,
                                    m => m.MovieId,
                                    (lm, m) => new { LikedMovie = lm, Movie = m }
                                    )
                                .Where(z => z.LikedMovie.UserId == userId)
                                .Where(z => z.LikedMovie.MovieId == z.Movie.MovieId)
                                .Select(e => new Movies
                                {
                                    MovieId = e.Movie.MovieId,
                                    Genre = e.Movie.Genre,
                                    Year = e.Movie.Year,
                                    Director = e.Movie.Director,
                                    Title = e.Movie.Title,
                                    RunTime = e.Movie.RunTime
                                })
                                .ToList();
           
            return likedMovies; 
        }
    }
}
