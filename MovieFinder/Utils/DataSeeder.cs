using Microsoft.EntityFrameworkCore;
using MovieFinder.Enum;
using MovieFinder.Models;
using System.Linq;

namespace MovieFinder.Utils
{
      public static class DataSeeder
    {
        public static void SeedData(MovieFinderContext context)
        {
            context.Database.Migrate();

            if (!context.RateLimits.Any(c => c.RateLimitId == RateLimitsEnum.ImdbAlternative))
            {
                var rateLimit = new RateLimits(RateLimitsEnum.ImdbAlternative, 1000);
                context.RateLimits.Add(rateLimit);
            }

            if (!context.RateLimits.Any(c => c.RateLimitId == RateLimitsEnum.Utelly))
            {
                var rateLimit = new RateLimits(RateLimitsEnum.Utelly, 1000);
                context.RateLimits.Add(rateLimit);
            }

            context.SaveChanges();
        }
    }
}
