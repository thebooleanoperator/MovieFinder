using MovieFinder.Models;

namespace MovieFinder.Repository.Interface
{
    public interface IGenresRepository : IMovieFinderRepository<Genres>
    {
        Genres GetByMovieId(int movieId);
    }
}
