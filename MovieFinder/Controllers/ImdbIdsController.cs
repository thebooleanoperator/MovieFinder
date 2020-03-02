using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using MovieFinder.Utils;
using System.Linq;
using System.Net.Http;
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
        //[Authorize]
        public async Task<IActionResult> GetByTitle([FromQuery] string title)
        {
            if (title == null || title.Length == 0)
            {
                return NoContent();
            }

            var imdbIds = _unitOfWork.ImdbIds.GetByTitle(title).ToList();

            if (imdbIds == null || imdbIds.Count() == 0)
            {
                var imdbIdsFromRapid = await _moviesService.GetImdbIdsFromTitle(title, null);

                if (imdbIdsFromRapid == null || imdbIdsFromRapid.Count() == 0)
                {
                    return NoContent();
                }

                foreach(var imdbId in imdbIdsFromRapid)
                {
                    var exisitingId = _unitOfWork.ImdbIds.GetByImdbId(imdbId.ImdbId);
                    if (exisitingId == null)
                    {
                        _unitOfWork.ImdbIds.Add(exisitingId);
                    }
                }

                _unitOfWork.SaveChanges();
                return Ok(imdbIdsFromRapid);
            }

            return Ok(imdbIds); 
            
        }


    }
}
