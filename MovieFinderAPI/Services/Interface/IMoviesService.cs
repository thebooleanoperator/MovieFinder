using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IMoviesService
    {
        /// <summary>
        /// Gets all movie info from from movie-database-alternative
        /// </summary>
        /// <param name="imdbId"></param>
        /// <returns></returns>
        Task<RapidMovieDto> GetMovieInfo([FromBody] ImdbIds imdbId);

        /// <summary>
        /// Returns a moviesDto filled out with Synopsis, Genre, and Streaming Data info.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        MoviesDto GetCompleteMovie(Movies movie);
    }
}
