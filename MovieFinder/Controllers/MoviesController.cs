using Microsoft.AspNetCore.Mvc;
using MovieFinder.Models;
using MovieFinder.Partialscs;
using MovieFinder.Repository;
using System.Linq;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class MoviesController : Controller
    {
        private readonly UnitOfWork _unitOfWork; 

        public MoviesController(MovieFinderContext MovieFinderContext)
        {
            _unitOfWork = new UnitOfWork(MovieFinderContext); 
        }

        [HttpPost]
        public IActionResult Create([FromBody] MoviesDto moviesDto)
        {
            var movie = new Movies(moviesDto); 

            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges(); 

            return Ok(movie); 
        }

        [HttpGet]
        [Route("{movieId}")]
        public IActionResult Get(int movieId)
        {
            var movie = _unitOfWork.Movies.Get(movieId);

            return Ok(movie); 
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var movies = _unitOfWork.Movies.GetAll().ToList();
            return Ok(movies);
        }

        [HttpDelete]
        [Route("{movieId}")]
        public IActionResult Delete(int movieId)
        {
            var movie = _unitOfWork.Movies.Get(movieId);

            _unitOfWork.Movies.Remove(movie);
            _unitOfWork.SaveChanges();

            return Ok(); 
        }
    }
}
