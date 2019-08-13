using Microsoft.AspNetCore.Mvc;
using MovieFinder.Models;
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

        [HttpPost]
        public IActionResult Create([FromBody] LikedMovies likedMovies)
        {
            _unitOfWork.LikedMovies.Add(likedMovies);
            _unitOfWork.SaveChanges();

            return Ok(likedMovies);
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
