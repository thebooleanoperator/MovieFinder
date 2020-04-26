using MovieFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Repository.Interface
{
    public interface ILikedMoviesRepository : IMovieFinderRepository<LikedMovies>
    {
        List<Movies> GetAll(int userId);  
    }
}
