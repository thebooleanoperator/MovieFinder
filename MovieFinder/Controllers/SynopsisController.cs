using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost]
        public IActionResult Create([FromBody]Synopsis synopsis)
        {
            _unitOfWork.Synopsis.Add(synopsis);
            _unitOfWork.SaveChanges();

            return Ok(synopsis); 
        }

        [HttpGet("{movieId}")]
        public IActionResult GetByMovieId(int movieId)
        {
            var synopsis = _unitOfWork.Synopsis.GetByMovieId(movieId); 

            return Ok(synopsis); 
        }
    }
}
