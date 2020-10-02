using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface ILikedMoviesRepository : IMovieFinderRepository<LikedMovies>
    {
        LikedMovies GetByMovieId(int movieId, int userId);
        IEnumerable<LikedMovies> GetAll(int userId, int? skip = null, int? count = null);
    }
}
