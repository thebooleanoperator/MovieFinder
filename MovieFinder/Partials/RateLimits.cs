using MovieFinder.Enum;
using MovieFinder.Enum.Validators;
using System;

namespace MovieFinder.Models
{
    public partial class RateLimits
    {
        public RateLimits()
        {
            
        }

        public RateLimits(RateLimitsEnum rateLimitsEnum, int initialLimit)
        {
            if (!RateLimitsValidator.IsValidRateLimits(rateLimitsEnum))
            {
                throw new ArgumentException($"{nameof(rateLimitsEnum)} must be a valid rate limit.");
            }

            if (initialLimit < 0)
            {
                throw new ArgumentException($"{nameof(initialLimit)} cannot be a negative numer.");
            }

            RateLimitId = rateLimitsEnum;
            RequestsRemaining = initialLimit;
        }
    }
}
