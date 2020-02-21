using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface IImdbIdsRepository : IMovieFinderRepository<ImdbIds>
    {
        ImdbIds GetByImdbId(string imdbId);
        IEnumerable<ImdbIds> GetByTitle(string title);
    }
}
