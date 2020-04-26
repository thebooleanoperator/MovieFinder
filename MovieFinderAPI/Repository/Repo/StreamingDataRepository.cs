using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class StreamingDataRepository : MovieFinderRepository<StreamingData>, IStreamingDataRepository
    {
        public StreamingDataRepository(DbContext context) : base(context)
        {

        }

        public StreamingData GetByMovieId(int movieId)
        {
            return DbSet.Where(sd => sd.MovieId == movieId).Single();
        }
    }
}
