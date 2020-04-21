using System.Linq;

namespace MovieFinder.Enum.Validators
{
    public static class RateLimitsValidator
    {
        public static int minLimit = System.Enum.GetValues(typeof(RateLimitsEnum)).Cast<int>().Min();
        public static int maxLimit = System.Enum.GetValues(typeof(RateLimitsEnum)).Cast<int>().Max();

        public static bool IsValidRateLimits(RateLimitsEnum rateLimitEnum)
        {
            if ((int)rateLimitEnum < minLimit || (int)rateLimitEnum > maxLimit)
            {
                return false;
            }
            // The enum falls between the valid range. 
            return true;
        }
    }
}
