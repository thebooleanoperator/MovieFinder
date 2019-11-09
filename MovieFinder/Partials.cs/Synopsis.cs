using MovieFinder.DtoModels;
using System;
using System.Linq;

namespace MovieFinder.Models
{
    public partial class Synopsis
    {
        public Synopsis(SynopsisDto synopsisDto, IQueryable<Synopsis> allSynopsis)
        {
            if(synopsisDto.Plot.Length == 0)
            {
                throw new ArgumentException($"{synopsisDto.Plot} must have characters");
            }

            if(synopsisDto.MovieId <= 0)
            {
                throw new ArgumentException($"{synopsisDto.MovieId} must be greater than 0");
            }

            //Not sure if this is needed, Foreign key MovieId may handle this. 
            if (allSynopsis.Any(m => m.MovieId == synopsisDto.MovieId))
            {
                throw new ArgumentException($"Movie Id {synopsisDto.MovieId} already exists and has a synopsis");
            }

            Plot = synopsisDto.Plot;
            MovieId = synopsisDto.MovieId;
        }

        public void Update(SynopsisDto synopsisDto)
        {
            if (synopsisDto.Plot.Length == 0)
            {
                throw new ArgumentException($"The summary: {synopsisDto.Plot} must have characters");
            }

            if (synopsisDto.MovieId <= 0)
            {
                throw new ArgumentException($"MovieId is {synopsisDto.MovieId}, must be greater than 0");
            }

            Plot = synopsisDto.Plot;
            MovieId = synopsisDto.MovieId;
        }
    }
}
