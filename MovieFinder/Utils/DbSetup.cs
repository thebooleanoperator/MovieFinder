using System;
using Microsoft.Extensions.DependencyInjection;
using MovieFinder.Repository;

namespace MovieFinder.Utils
{
    public class DbSetup
    {
        private MovieFinderContext _dbContext; 

        public DbSetup(IServiceProvider service)
        {
            _dbContext = service.GetService<MovieFinderContext>();
        }

        public static void MoviesInit()
        {
           
        }
    }
}
