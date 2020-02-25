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

        public ImdbIdsController(MovieFinderContext movieFinderContext, IHttpClientFactory clientFactory)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _moviesService = new MoviesService(clientFactory, _unitOfWork);

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

            if (imdbIds == null || imdbIds.Count() == 0)
            {
                var imdbIdsFromRapid = await _moviesService.GetImdbIdsFromTitle(title, null);
                return Ok(imdbIdsFromRapid);
            }

            return Ok(imdbIds); 
            
        }


    }
}
