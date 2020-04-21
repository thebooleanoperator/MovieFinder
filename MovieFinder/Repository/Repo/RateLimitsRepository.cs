using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;

namespace MovieFinder.Repository.Repo
{
    public class RateLimitsRepository : MovieFinderRepository<RateLimits>, IRateLimitsRepository 
    {
        public RateLimitsRepository(DbContext context) : base(context)
        {

        }
    }
}
