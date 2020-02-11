using MovieFinder.Models;

namespace MovieFinder.Repository.Interface
{
    public interface IStreamingDataRepository : IMovieFinderRepository<StreamingData>
    {
        StreamingData GetByMovieId(int movieId);
    }
}
