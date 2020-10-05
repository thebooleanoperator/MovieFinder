using MovieFinder.DtoModels;
using MovieFinder.Models;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IStreamingDataService
    {
        /// <summary>
        /// Gets all streaming data for a movie from Utelly API. 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<RapidStreamingDto> GetStreamingData(string imdbId);

        /// <summary>
        /// Updates StreamingData for a movie if it has not been updated in last 7 days.
        /// </summary>
        /// <param name="moviesDto"></param>
        /// <returns></returns>
        Task<StreamingData> GetUpdatedStreamingData(StreamingData streamingData, string imdbId);
    }
}
