using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface IMoviesRepository : IMovieFinderRepository<Movies>
    {
        Movies GetByImdbId(string imdbId);
        IEnumerable<Movies> GetAllByTitle(string title);
        IEnumerable<Movies> GetAllRecommended();
        IEnumerable<Movies> Get(IEnumerable<int> movieIds);
    }
}
