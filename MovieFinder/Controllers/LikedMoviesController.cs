﻿using Microsoft.AspNetCore.Mvc;
using MovieFinder.Models;

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
        public IActionResult GetAllByUserId(int userId)
        {
            var likedMovies =_unitOfWork.LikedMovies.GetAll(userId);

            return Ok(likedMovies); 
        }
    }
}
