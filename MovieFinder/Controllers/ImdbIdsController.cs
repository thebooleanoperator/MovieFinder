using Microsoft.AspNetCore.Authorization;
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

            var existingImdbIds = _unitOfWork.ImdbIds.GetByTitle(title, year).ToList();

            // If there is an exact match get all the close matches and return them. 
            if (existingImdbIds != null && existingImdbIds.Count() > 0)
            {
                var closelyMatchingImdbIds = _unitOfWork.ImdbIds.GetByTitle(title, year, false); 
                return Ok(closelyMatchingImdbIds.OrderByDescending(i => i.Year));
            }

            var imdbIdsFromRapid = await _moviesService.GetImdbIdsByTitle(title, year);
            // If there were no imdbIds found on inital search, search none rate limited imdb api for movies.
            // There's no way to specify a year to the backup API, ignore the year. 
            if (imdbIdsFromRapid == null)
            {
                imdbIdsFromRapid = new List<ImdbIds>();
                // Use movieService to call backup API.
                var idsFromRapid = await _moviesService.GetOnlyIdByTitle(title);
                List<ImdbIds> imdbIdsFromRApid = new List<ImdbIds>();

                foreach (var id in idsFromRapid)
                {
                    // In order to get the year, we need to use the rate limited imdb api.
                    var imdbId = await _moviesService.GetImdbIdById(id.Id);
                    // imdbId will be null when parsing fails.
                    if (imdbId != null) { imdbIdsFromRapid.Add(imdbId); }
                }
            }

            // There was not movie with title found. Try to get closely matched results. 
            if (imdbIdsFromRapid == null || imdbIdsFromRapid.Count() == 0)
            {
                var closelyMatchingImdbIds = _unitOfWork.ImdbIds.GetByTitle(title, year, false);

                // If close matches are empty, return NotFound. 
                if (closelyMatchingImdbIds == null || closelyMatchingImdbIds.Count() == 0) { return NotFound(); }

                return Ok(closelyMatchingImdbIds); 
            }

            foreach (var imdbId in imdbIdsFromRapid)
            {
                var exisitingId = _unitOfWork.ImdbIds.Get(imdbId.ImdbId);
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
