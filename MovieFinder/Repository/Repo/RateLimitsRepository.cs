using Microsoft.EntityFrameworkCore;
using MovieFinder.Enum;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class RateLimitsRepository : MovieFinderRepository<RateLimits>, IRateLimitsRepository 
    {
        public RateLimitsRepository(DbContext context) : base(context)
        {

        }

        public RateLimits GetImdbAlternative()
        {
            return DbSet.First(r => r.RateLimitId == RateLimitsEnum.ImdbAlternative); 
        }

        public RateLimits GetUtelly()
        {
            return DbSet.First(r => r.RateLimitId == RateLimitsEnum.Utelly);
        }
    }
}
