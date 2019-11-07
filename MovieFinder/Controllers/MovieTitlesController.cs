using Microsoft.AspNetCore.Mvc;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class MovieTitlesController : Controller
    {
        private UnitOfWork _unitOfWork; 

        public MovieTitlesController(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        [HttpPost]
        public IActionResult SetDatabase()
        {
            var titles = DataReader.ReadMovies("https://raw.githubusercontent.com/prust/wikipedia-movie-data/master/movies.json"); 

            foreach(var title in titles)
            {
                var movieTitles = new MovieTitles(title); 
                _unitOfWork.MovieTitles.Add(movieTitles); 
            }
            _unitOfWork.SaveChanges();

            return Ok();
        }
    }
}
