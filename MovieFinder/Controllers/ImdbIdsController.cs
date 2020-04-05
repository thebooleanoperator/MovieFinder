﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class ImdbIdsController : Controller
    {
        private UnitOfWork _unitOfWork;
        private IMoviesService _moviesService; 

        public ImdbIdsController(MovieFinderContext movieFinderContext, IMoviesService moviesService)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _moviesService = moviesService;
        }

        /// <summary>
        /// Gets a list of ImdbIds by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByTitle([FromQuery] string title, [FromQuery] int? year = null)
        {
            if (title == null || title.Length == 0)
            {
                return NoContent();
            }

            var existingImdbIds = _unitOfWork.ImdbIds.GetByTitleAndYear(title, year).ToList();

            // If there is an exact matching imdbIds return them.
            if (existingImdbIds != null && existingImdbIds.Count() > 0)
            {
                return Ok(existingImdbIds.OrderByDescending(i => i.Year));
            }

            var imdbIdsFromRapid = await _moviesService.GetImdbIdsFromTitle(title, year);
            // If there were no imdbIds found on inital search, search backupApi for movies.
            // There's no way to specify a year to the backup API, skip if user requests specific year.
            if (imdbIdsFromRapid == null && year == null)
            {
                imdbIdsFromRapid = new List<ImdbIds>();
                var idsFromRapid = await _moviesService.GetIdsFromTitle(title);
                List<ImdbIds> imdbIdsFromRApid = new List<ImdbIds>();

                foreach (var id in idsFromRapid)
                {
                    var imdbId = await _moviesService.GetImdbIdById(id.Id);
                    // imdbId will be null when parsing fails.
                    if (imdbId != null) { imdbIdsFromRapid.Add(imdbId); }
                }
            }

            // There was not movie with title found.
            if (imdbIdsFromRapid == null || imdbIdsFromRapid.Count() == 0)
            {
                return NotFound();
            }

            foreach (var imdbId in imdbIdsFromRapid)
            {
                var exisitingId = _unitOfWork.ImdbIds.GetByImdbId(imdbId.ImdbId);
                if (exisitingId == null)
                {
                    // Add and save everytime to avoid adding duplicate pk into db.
                    _unitOfWork.ImdbIds.Add(imdbId);
                    _unitOfWork.SaveChanges();
                }
            }

            return Ok(imdbIdsFromRapid.OrderByDescending(i => i.Year));
        }
    }
}
