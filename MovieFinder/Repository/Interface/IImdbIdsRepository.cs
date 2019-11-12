using MovieFinder.Models;

namespace MovieFinder.Repository.Interface
{
    public interface IImdbIdsRepository : IMovieFinderRepository<ImdbIds>
    {
        ImdbIds GetByString(string imdbId);
    }
}
