using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface IMovieTitlesRepository : IMovieFinderRepository<MovieTitles>
    {
        bool MovieTitleExists(string title, int year);
        IEnumerable<MovieTitles> GetNext(int page, int count);
    }
}
