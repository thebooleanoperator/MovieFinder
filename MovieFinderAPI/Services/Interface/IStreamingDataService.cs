using MovieFinder.DtoModels;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IStreamingDataService
    {
        Task UpdateStreamingData(MoviesDto moviesDto);
    }
}
