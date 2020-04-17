using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface IImdbIdsRepository : IMovieFinderRepository<ImdbIds>
    {
        /// <summary>
        /// Gets ImdbId with imdbId; 
        /// </summary>
        ImdbIds Get(string imdbId);

        /// <summary>
        /// Gets ImdbId with title and optional year. 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        IEnumerable<ImdbIds> GetByTitle(string title, int? year, bool exactMatch = true);
    }
}
