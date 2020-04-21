using MovieFinder.Enum;
using MovieFinder.Models;
using System.Linq;

namespace MovieFinder.Utils
{
      public static class DataSeeder
    {
        public static void SeedRateLimits(MovieFinderContext context)
        {
            if (!context.RateLimits.Any(c => c.RateLimitId == (int)RateLimitsEnum.ImdbAlternative))
            {
                var rateLimit = new RateLimits(RateLimitsEnum.ImdbAlternative, 1000);
                context.RateLimits.Add(rateLimit);
            }

            if (!context.RateLimits.Any(c => c.RateLimitId == (int)RateLimitsEnum.Utelly))
            {
                var rateLimit = new RateLimits(RateLimitsEnum.Utelly, 1000);
                context.RateLimits.Add(rateLimit);
            }

            context.SaveChanges();
        }
    }
}
