using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Repository.Repo
{
    [Route("{controller}")]
    public class LikedMoviesController : Controller 
    {
        private UnitOfWork _unitOfWork; 

        public LikedMoviesController(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext); 
        }

        public IActionResult Create(int userId, int movieId)
        {
            
        }

        [HttpGet]
        [Route("{userId}")]
        public IActionResult GetAll(int userId)
        {
            var likedMovies =_unitOfWork.LikedMovies.GetAll(userId);

            return Ok(likedMovies); 
        }
    }
}
