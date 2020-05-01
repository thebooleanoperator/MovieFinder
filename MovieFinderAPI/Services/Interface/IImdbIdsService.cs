using MovieFinder.DtoModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IImdbIdsService
    {
        /// <summary>
        /// Gets ImdbId (id, title, year) from RapidAPI movie-database-alternative. 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        Task<List<RapidImdbDto>> GetImdbIdsByTitle(string title, int? year);

        /// <summary>
        /// Gets an imdbId objcet by an imdbId id. Need to use this to get the year 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RapidImdbDto> GetImdbIdById(string imdbId);

        /// <summary>
        /// Gets only the title and imdbId from unoffical Imdb API. 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<List<RapidImdbDto>> GetOnlyIdByTitle(string title);
    }
}
