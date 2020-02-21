using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class ImdbIdsController : Controller
    {
        private UnitOfWork _unitOfWork; 

        public ImdbIdsController(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetMoviesByTitle([FromQuery] string title)
        {
            if (title == null || title.Length == 0)
            {
                return NoContent();
            }

            var imdbIds = _unitOfWork.Movies.GetAllByTitle(title).ToList();

            if (imdbIds == null || imdbIds.Count() == 0)
            {
                ImdbIds =
            }
            else
            {
                return Ok(imdbIds); 
            }
        }

    }
}
