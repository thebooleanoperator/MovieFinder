using MovieFinder.Enum;

namespace MovieFinder.Services.Interface
{
    public interface IRateLimitsService
    {
        bool IsRequestsRemaining(RateLimitsEnum rateLimits);
    }
}
