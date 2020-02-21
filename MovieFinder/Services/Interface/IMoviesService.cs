using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IMoviesService
    {
        Task<ImdbInfoDto> GetImdbMovieInfo([FromBody] ImdbIds imdbId);
        Task<List<ImdbIds>> GetImdbIdsFromTitle(string title, int? year);
    }
}
