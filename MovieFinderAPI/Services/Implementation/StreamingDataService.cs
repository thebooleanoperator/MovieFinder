using MovieFinder.DtoModels;
using MovieFinder.Enum;
using MovieFinder.Repository;
using MovieFinder.Services.Interface;
using System;
using System.Threading.Tasks;

namespace MovieFinder.Services.Implementation
{
    public class StreamingDataService : IStreamingDataService 
    {
        private UnitOfWork _unitOfWork;
        private IRateLimitsService _rateLimitsService; 
        private IMoviesService _moviesService;

        public StreamingDataService(MovieFinderContext movieFinderContext, IMoviesService moviesService, IRateLimitsService rateLimitsService)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
            _rateLimitsService = rateLimitsService;
            _moviesService = moviesService;
        }

        public async Task UpdateStreamingData(MoviesDto moviesDto)
        {
            var lastUpdated = moviesDto.StreamingData.LastUpdated;
            var needsUpdate = DateTime.Now.Subtract(lastUpdated).Days <= 7 ? false : true;

            if (needsUpdate == true)
            {
                var updatedStreamingData = await _moviesService.GetStreamingData(moviesDto.Title, moviesDto.ImdbId);

                moviesDto.StreamingData.Patch(updatedStreamingData);

                _unitOfWork.StreamingData.Update(moviesDto.StreamingData);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
