using MovieFinder.Enum;
using MovieFinder.Enum.Validators;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using System;
using System.Threading.Tasks;

namespace MovieFinder.Services.Implementation
{
    public class RateLimitsService : IRateLimitsService 
    {
        private readonly UnitOfWork _unitOfWork;
        private RateLimits imdbAlternative;
        private RateLimits utelly; 

        public RateLimitsService(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            InitRateLimits(); 
        }

        private void InitRateLimits()
        {
            imdbAlternative = _unitOfWork.RateLimits.GetImdbAlternative();
            utelly = _unitOfWork.RateLimits.GetUtelly(); 
        }

        public async Task Update(RateLimitsEnum rateLimitsEnum, int requestsRemaining)
        {
            if (!RateLimitsValidator.IsValidRateLimits(rateLimitsEnum))
            {
                throw new ArgumentException($"{nameof(rateLimitsEnum)} is not valid.");
            }

            switch (rateLimitsEnum)
            {
                case RateLimitsEnum.ImdbAlternative:
                    imdbAlternative.Patch(requestsRemaining);
                    _unitOfWork.RateLimits.Update(imdbAlternative); 
                    break;

                case RateLimitsEnum.Utelly:
                    utelly.Patch(requestsRemaining);
                    _unitOfWork.RateLimits.Update(utelly);
                    break;
            }
            
            await _unitOfWork.SaveChangesAsync(); 
        }

        public bool IsRequestsRemaining(RateLimitsEnum rateLimitsEnum)
        {
            if (!RateLimitsValidator.IsValidRateLimits(rateLimitsEnum))
            {
                throw new ArgumentException($"{nameof(rateLimitsEnum)} is not valid.");
            }

            switch (rateLimitsEnum)
            {
                case RateLimitsEnum.ImdbAlternative:
                    return imdbAlternative.RequestsRemaining > 0;

                case RateLimitsEnum.Utelly:
                    return utelly.RequestsRemaining > 0;

                default:
                    return false;
            }
        }
    }
}
