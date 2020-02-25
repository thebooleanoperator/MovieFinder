using MovieFinder.Models;
using System;

namespace MovieFinder.DtoModels
{
    public class MovieSearchDto
    {
        public Movies Movie { get; set; }
        public StreamingData StreamingData { get; set; }
        public Synopsis Synopsis { get; set; }

        public MovieSearchDto()
        {
        }

        public MovieSearchDto(Movies movie, StreamingData streamingData, Synopsis synopsis)
        {
            Movie = movie ?? throw new ArgumentException("Movie cannot be null.");
            StreamingData = streamingData ?? throw new ArgumentException("Streaming Data cannot be null.");
            Synopsis = synopsis ?? throw new ArgumentException("Synopsis cannot be null.");
        }
    }
}
