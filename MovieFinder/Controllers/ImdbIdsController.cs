using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetByTitle([FromQuery] string title)
        {
            if (title == null || title.Length == 0)
            {
                return NoContent();
            }

            var imdbIds = _unitOfWork.ImdbIds.GetByTitle(title).ToList();

            // If there is an exact matching imdbIds return them.
            if (imdbIds != null && imdbIds.Count() > 0)
            {
                return Ok(imdbIds.OrderByDescending(i => i.Year));
            }

            var imdbIdsFromUtelly = await _moviesService.GetImdbIdsFromTitle(title, null);
            // If Utelly returns ImdbIds, add any that don't exist in the database and return in descending order by year.
            if (imdbIdsFromUtelly != null && imdbIdsFromUtelly.Count() > 0)
            {
                foreach (var imdbId in imdbIdsFromUtelly)
                {
                    var exisitingId = _unitOfWork.ImdbIds.GetByImdbId(imdbId.ImdbId);
                    if (exisitingId == null)
                    {
                        // Add and save everytime to avoid adding duplicate pk into db.
                        _unitOfWork.ImdbIds.Add(imdbId);
                        _unitOfWork.SaveChanges();
                    }
                }

                return Ok(imdbIdsFromUtelly.OrderByDescending(i => i.Year));
            }

            return NotFound($"Could not find any movies that match {title}");
        }
    }
}
