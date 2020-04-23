using MovieFinder.Enum;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IRateLimitsService
    {
        /// <summary>
        /// Returns a truthy value if the RateLimitsEnum has requests remaining. 
        /// </summary>
        /// <param name="rateLimits"></param>
        /// <returns></returns>
        bool IsRequestsRemaining(RateLimitsEnum rateLimits);

        /// <summary>
        /// Updates a RateLimitsEnum with a new remainingRequests value. 
        /// </summary>
        /// <param name="rateLimitsEnum"></param>
        /// <param name="requestsRemaining"></param>
        /// <returns></returns>
        Task Update(RateLimitsEnum rateLimitsEnum, int requestsRemaining);
    }
}
