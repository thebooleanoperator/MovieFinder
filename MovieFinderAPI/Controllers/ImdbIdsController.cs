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
        private IImdbIdsService _imdbIdsService; 

        public ImdbIdsController(MovieFinderContext movieFinderContext, IImdbIdsService imdbIdsService)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _imdbIdsService = imdbIdsService;
        }

        /// <summary>
        /// Gets a list of ImdbIds by title by search and year.
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

            title = title.Trim();

            var existingImdbIds = _unitOfWork.ImdbIds.GetByTitle(title, year).ToList();
            var closelyMatchingImdbIds = _unitOfWork.ImdbIds.GetByTitle(title, year, false);
            var closelyMatchRapidDtos = RapidImdbDto.ConvertFromImdbIds(closelyMatchingImdbIds); 

            // If there is an exact match do not send requests. 
            if (existingImdbIds != null && existingImdbIds.Count() > 0)
            {
                return Ok(closelyMatchRapidDtos.OrderByDescending(i => i.Year));
            }
            // Inital api request to get imdbIds.
            var rapidDtos = await _imdbIdsService.GetImdbIdsByTitle(title, year);

            // If there were no imdbIds found on inital search, search none rate limited imdb api for movies.
            // There's no way to specify a year to the backup API, ignore the year. 
            if (rapidDtos == null || rapidDtos.Count() ==0)
            {
                rapidDtos = new List<RapidImdbDto>();
                // 2nd api request to get ImdbIds using backup API. This will return only the ImdbId and Title, no Years. 
                var partialRapidDtos = await _imdbIdsService.GetOnlyIdByTitle(title);
                foreach (var partialRapidDto in partialRapidDtos)
                {
                    var exisitngImdbId = _unitOfWork.ImdbIds.Get(partialRapidDto.ImdbId); 
                    if (exisitngImdbId != null && (year == null || exisitngImdbId.Year == year))
                    {
                        var rapidDtoFromExistingImdbId = new RapidImdbDto(exisitngImdbId);

                        rapidDtos.Add(rapidDtoFromExistingImdbId);
                    }
                    if (exisitngImdbId == null)
                    {
                        // 3rd api request using primary API to get the complete ImdbId, a use the rate limited imdb api.
                        var rapidDto = await _imdbIdsService.GetImdbIdById(partialRapidDto.Id);
                        // imdbId will be null when parsing fails or the API requests limit is reached.
                        if (rapidDto != null && (year == null || rapidDto.Year == year))
                        {
                            rapidDtos.Add(rapidDto);
                        }
                    }
                }
            }

            // There was not movie with title found. Try to get closely matched results. 
            if (rapidDtos == null || rapidDtos.Count() == 0)
            {
                var closeMatches = closelyMatchingImdbIds.OrderByDescending(i => i.Year);
                if (year == null && closeMatches.Count() > 0)
                {
                    return Ok(closeMatches);
                }
                return NoContent();
            }

            foreach (var rapidDto in rapidDtos)
            {
                var exisitingId = _unitOfWork.ImdbIds.Get(rapidDto.ImdbId);
                if (exisitingId == null)
                {
                    // Add and save everytime to avoid adding duplicate pk into db when rapidDtos contains duplicates.
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
