using MovieFinder.DtoModels;
using System;
using System.Linq;

namespace MovieFinder.Models
{
    public partial class Synopsis
    {
        public Synopsis()
        {

        }

        public Synopsis(SynopsisDto synopsisDto, IQueryable<Synopsis> allSynopsis)
        {
            if(synopsisDto.SynopsisSummary.Length == 0)
            {
                throw new ArgumentException($"{synopsisDto.SynopsisSummary} must have characters");
            }

            if(synopsisDto.MovieId <= 0)
            {
                throw new ArgumentException($"{synopsisDto.MovieId} must be greater than 0");
            }

            if (allSynopsis.Any(m => m.MovieId == synopsisDto.MovieId))
            {
                throw new ArgumentException($"Movie Id {synopsisDto.MovieId} already exists and has a synopsis");
            }

            SynopsisSummary = synopsisDto.SynopsisSummary;
            MovieId = synopsisDto.MovieId;
        }
    }
}
