﻿using MovieFinder.DtoModels;

namespace MovieFinder.Models
{
    public partial class StreamingData
    {
        public StreamingData()
        {

        }

        public StreamingData(RapidStreamingDto rapidStreamingData, Movies movie)
        {

            MovieId = movie.MovieId;
            Netflix = OnApp(rapidStreamingData, "netflix");
            HBO = OnApp(rapidStreamingData, "hbo");
            Hulu = OnApp(rapidStreamingData, "hulu");
            DisneyPlus = OnApp(rapidStreamingData, "disney");
            AmazonPrime = OnApp(rapidStreamingData, "prime");
            ITunes = OnApp(rapidStreamingData, "itunes");
            GooglePlay = OnApp(rapidStreamingData, "google"); 
        }

        public string OnApp(RapidStreamingDto rapidStreamingData, string appName)
        {
            if (rapidStreamingData == null) { return null; }
            foreach(var location in rapidStreamingData.Locations)
            {
                if (location.Display_Name.ToLower().Contains(appName))
                {
                    return location.Icon;
                }
            }
            return null;
        }
    }
}