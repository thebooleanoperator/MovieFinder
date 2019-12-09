using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Repository;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class GenresController : Controller
    {
        private UnitOfWork _unitOfWork; 

        public GenresController(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        [HttpGet("{movieId}")]
        public IActionResult GetByMovieId(int movieId)
        {
            var genres = _unitOfWork.Genres.GetByMovieId(movieId);
            var genreDto = new GenresDto(genres); 
 
            return Ok(genreDto);
        }
    }
}