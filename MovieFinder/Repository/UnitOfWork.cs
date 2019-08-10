﻿using Microsoft.EntityFrameworkCore;
using MovieFinder.Repository.Interface;
using MovieFinder.Repository.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Repository
{
    public class UnitOfWork
    {
        public IMoviesRepository Movies { get; set; }
        public ILikedMoviesRepository LikedMovies { get; set; }
       
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        
            Movies = new MoviesRepository(_context);
            LikedMovies = new LikedMoviesRepository(_context); 
        }

        public int SaveChanges()
        {
            return _context.SaveChanges(); 
        }
    }
}
