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

        /// <summary>
        /// Takes a list of likedMoves, and returns a list of complete movies, with updated streaming data.
        /// </summary>
        /// <param name="likedMovies"></param>
        /// <returns></returns>
        List<MoviesDto> GetCompleteLikedMovies(List<LikedMovies> likedMovies);
    }
}
