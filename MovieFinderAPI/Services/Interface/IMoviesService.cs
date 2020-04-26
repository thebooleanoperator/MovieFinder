﻿using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IMoviesService
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

        /// <summary>
        /// Gets all movie info from from movie-database-alternative
        /// </summary>
        /// <param name="imdbId"></param>
        /// <returns></returns>
        Task<RapidMovieDto> GetMovieInfo([FromBody] ImdbIds imdbId);

        /// <summary>
        /// Gets all streaming data for a movie from Utelly API. 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        Task<RapidStreamingDto> GetStreamingData(string title, string imdbId);

        /// <summary>
        /// Returns a moviesDto filled out with Synopsis, Genre, and Streaming Data info.
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        MoviesDto GetCompleteMovie(Movies movie);
    }
}