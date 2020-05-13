using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface ILikedMoviesRepository : IMovieFinderRepository<LikedMovies>
    {
        IEnumerable<LikedMovies> GetAllByUserId(int userId, int? page, int? count);
    }
}
