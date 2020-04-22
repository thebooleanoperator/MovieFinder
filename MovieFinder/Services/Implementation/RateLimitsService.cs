using MovieFinder.Repository;
using MovieFinder.Services.Interface;

namespace MovieFinder.Services.Implementation
{
    public class RateLimitsService : IRateLimitsService 
    {
        private readonly UnitOfWork _unitOfWork; 

        public RateLimitsService(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }


    }
}
