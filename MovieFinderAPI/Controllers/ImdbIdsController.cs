using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
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
            var closelyMatchingImdbIds = _unitOfWork.ImdbIds.GetByTitle(title, year, false);
            var closelyMatchRapidDtos = RapidImdbDto.ConvertFromImdbIds(closelyMatchingImdbIds); 

            // If there is an exact match do not send requests. 
            if (existingImdbIds != null && existingImdbIds.Count() > 0)
            {
                return Ok(closelyMatchRapidDtos.OrderByDescending(i => i.Year));
            }

            var rapidDtos = await _moviesService.GetImdbIdsByTitle(title, year);

            // If there were no imdbIds found on inital search, search none rate limited imdb api for movies.
            // There's no way to specify a year to the backup API, ignore the year. 
            if (rapidDtos == null || rapidDtos.Count() ==0)
            {
                rapidDtos = new List<RapidImdbDto>();
                // Use movieService to call backup API. This will return only the ImdbId and Title, no Years. 
                var partialRapidDtos = await _moviesService.GetOnlyIdByTitle(title);
                foreach (var partialRapidDto in partialRapidDtos)
                {
                    var exisitngImdbId = _unitOfWork.ImdbIds.Get(partialRapidDto.ImdbId); 
                    if (exisitngImdbId != null)
                    {
                        var rapidDtoFromExistingImdbId = new RapidImdbDto(exisitngImdbId); 
                        rapidDtos.Add(rapidDtoFromExistingImdbId);
                    }
                    else
                    {
                        // In order to get the Year, we need to use the rate limited imdb api.
                        var rapidDto = await _moviesService.GetImdbIdById(partialRapidDto.Id);
                        // imdbId will be null when parsing fails or the API requests limit is reached.
                        if (rapidDto == null)
                        {
                            partialRapidDto.ImdbId = partialRapidDto.Id;
                            rapidDtos.Add(partialRapidDto);
                        }
                        else
                        {
                            rapidDtos.Add(rapidDto);
                        }
                    }
                }
            }

            // There was not movie with title found. Try to get closely matched results. 
            if (rapidDtos == null || rapidDtos.Count() == 0)
            {
                return Ok(closelyMatchingImdbIds.OrderByDescending(i => i.Year));
            }

            foreach (var rapidDto in rapidDtos)
            {
                var exisitingId = _unitOfWork.ImdbIds.Get(rapidDto.ImdbId);
                if (exisitingId == null && string.IsNullOrEmpty(rapidDto.Id))
                {
                    // Add and save everytime to avoid adding duplicate pk into db.
                    var imdbId = new ImdbIds(rapidDto); 
                    _unitOfWork.ImdbIds.Add(imdbId);
                    _unitOfWork.SaveChanges();
                }
            }

            // Add the close matches to give users all possible movies.
            var allRapidDtos = RapidImdbDto.CombineWithNoDuplicates(rapidDtos, closelyMatchRapidDtos.ToList()); 

            return Ok(allRapidDtos.OrderByDescending(i => i.Year));
        }
    }
}
