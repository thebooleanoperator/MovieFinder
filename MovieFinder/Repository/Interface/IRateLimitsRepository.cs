using MovieFinder.Models;

namespace MovieFinder.Repository.Interface
{
    public interface IRateLimitsRepository : IMovieFinderRepository<RateLimits>
    {
        /// <summary>
        /// Gets the ImdbAlternative RateLimits entity.
        /// </summary>
        /// <returns></returns>
        RateLimits GetImdbAlternative();

        /// <summary>
        /// Gets the Utelly RateLimits entity.
        /// </summary>
        /// <returns></returns>
        RateLimits GetUtelly(); 
    }
}
