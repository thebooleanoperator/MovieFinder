using MovieFinder.DtoModels;
using System.Collections.Generic;

namespace MovieFinder.Models
{
    public partial class StreamingData
    {
        public StreamingData()
        {
        }

        public StreamingData(StreamingDataDto streamingDataDto, Movies movie)
        {

            MovieId = movie.MovieId;
            Netflix = OnApp(streamingDataDto, "netflix");
            HBO = OnApp(streamingDataDto, "hbo");
            Hulu = OnApp(streamingDataDto, "hulu");
            DisneyPlus = OnApp(streamingDataDto, "disney");
            AmazonPrime = OnApp(streamingDataDto, "prime");
            ITunes = OnApp(streamingDataDto, "itunes");
            GooglePlay = OnApp(streamingDataDto, "google"); ;
        }

        public bool OnApp(StreamingDataDto dataDto, string appName)
        {
            if (dataDto == null) { return false; }
            foreach(var location in dataDto.Locations)
            {
                if (location.Display_Name.ToLower().Contains(appName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
