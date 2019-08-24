using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class SynopsisController : Controller 
    {
        public UnitOfWork _unitOfWork; 

        public SynopsisController(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);  
        }

        /// <summary>
        /// Creates a Synopsis. 
        /// </summary>
        /// <param name="synopsis"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create([FromBody]SynopsisDto synopsisDto)
        {
            var allSynopsis = _unitOfWork.Synopsis.GetAll();

            var synopsis = new Synopsis(synopsisDto, allSynopsis);

            var movie = _unitOfWork.Movies.Get(synopsis.MovieId);

            if(movie == null)
            {
                return NotFound();
            }

            _unitOfWork.Synopsis.Add(synopsis);
            _unitOfWork.SaveChanges();

            return Ok(synopsis); 
        }

        /// <summary>
        /// Gets a Synopsis by the a movieId
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        [HttpGet("{movieId}")]
        public IActionResult GetByMovieId(int movieId)
        {
            var synopsis = _unitOfWork.Synopsis.GetByMovieId(movieId); 

            return Ok(synopsis); 
        }
    }
}
