using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Synopsis
    {
        public Synopsis()
        {

        }

        public Synopsis(SynopsisDto synopsisDto)
        {
            if(synopsisDto.SynopsisSummary.Length == 0)
            {
                throw new ArgumentException($"{synopsisDto.SynopsisSummary} must have characters");
            }

            if(synopsisDto.MovieId <= 0)
            {
                throw new ArgumentException($"{synopsisDto.MovieId} must be greater than 0");
            }

            SynopsisSummary = synopsisDto.SynopsisSummary;
            MovieId = synopsisDto.MovieId;
        }
    }
}
