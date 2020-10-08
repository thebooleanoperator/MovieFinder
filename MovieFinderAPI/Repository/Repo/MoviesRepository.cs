using Microsoft.EntityFrameworkCore;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using MovieFinder.Repository.Repo;
using System.Collections.Generic;
using System.Linq;

namespace MovieFinder.Repository
{
    public class MoviesRepository : MovieFinderRepository<Movies>, IMoviesRepository
    {
        DbContext _context; 

        public MoviesRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        new public Movies Get(int movieId)
        {
            return DbSet.Where(x => x.MovieId == movieId)
                    .Include(x => x.Genre)
                    .Include(x => x.StreamingData).SingleOrDefault();
        }

        public IEnumerable<Movies> Get(IEnumerable<int> movieIds)
        {
            return DbSet.Where(x => movieIds.Contains(x.MovieId));
        }

        public Movies GetByImdbId(string imdbId)
        {
            return DbSet.Where(m => m.ImdbId == imdbId)
                .Include(x => x.Genre)
                .Include(x => x.StreamingData)
                .SingleOrDefault();
        }

        public IEnumerable<Movies> GetAllByTitle(string title)
        {
            title = title.ToLower().Replace(" ", "");
            return DbSet.Where(m => m.Title.ToLower().Replace(" ", "").Contains(title));
        }

        public IEnumerable<RecommendedMoviesDto> GetAllRecommended(int userId)
        {
            var likedMovies = new LikedMoviesRepository(_context).GetAll().Where(x => x.UserId == userId);
            var recommendedMovies = DbSet.Where(x => x.IsRec == true)
                .Include(x => x.Genre)
                .Include(x => x.StreamingData);

            return from m in recommendedMovies
                   join lm in likedMovies
                   on m.MovieId equals lm.MovieId
                   into mov
                   from subMov in mov.DefaultIfEmpty()

                   select new RecommendedMoviesDto
                   {
                       MovieId = m.MovieId,
                       GenreId = m.GenreId,
                       StreamingDataId = m.StreamingDataId,
                       Year = m.Year,
                       Director = m.Director,
                       Title = m.Title,
                       ImdbRating = m.ImdbRating,
                       RottenTomatoesRating = m.RottenTomatoesRating,
                       ImdbId = m.ImdbId,
                       Plot = m.Plot,
                       Poster = m.Poster,
                       IsRec = m.IsRec,
                       RunTime = m.RunTime,
                       IsFavorite = mov.SingleOrDefault() == null ? false : true,
                       Genre = m.Genre,
                       StreamingData = m.StreamingData
                   };
        }
    }
}
