﻿using Microsoft.AspNetCore.Mvc;
using MovieFinder.Models;
using MovieFinder.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public IActionResult Create([FromBody] Movies movie)
        {
            _unitOfWork.Movies.Add(movie);
            _unitOfWork.SaveChanges(); 

            return Ok(movie); 
        }

        [HttpGet]
        public IActionResult Get(int movieId)
        {
            var movie = _unitOfWork.Movies.Get(movieId);

            return Ok(movie); 
        }
    }
}
