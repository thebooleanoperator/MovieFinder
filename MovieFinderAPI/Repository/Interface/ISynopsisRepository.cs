using MovieFinder.Models;

namespace MovieFinder.Repository.Interface
{
    public interface ISynopsisRepository : IMovieFinderRepository<Synopsis>
    {
        Synopsis GetByMovieId(int movieId);
    }
}
