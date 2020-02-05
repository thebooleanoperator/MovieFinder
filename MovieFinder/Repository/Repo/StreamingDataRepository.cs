using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;

namespace MovieFinder.Repository.Repo
{
    public class StreamingDataRepository : MovieFinderRepository<StreamingData>, IStreamingDataRepository
    {
        public StreamingDataRepository(DbContext context) : base(context)
        {

        }
    }
}
