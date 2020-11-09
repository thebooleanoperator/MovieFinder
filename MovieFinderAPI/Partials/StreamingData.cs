using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class StreamingData
    {
        public StreamingData()
        {

        }

        public StreamingData(RapidStreamingDto rapidStreamingData)
        {
            Netflix = OnApp(rapidStreamingData, "netflix");
            HBO = OnApp(rapidStreamingData, "hbo");
            Hulu = OnApp(rapidStreamingData, "hulu");
            DisneyPlus = OnApp(rapidStreamingData, "disney");
            AmazonPrime = OnApp(rapidStreamingData, "prime");
            ITunes = OnApp(rapidStreamingData, "itunes");
            GooglePlay = OnApp(rapidStreamingData, "google");
            LastUpdated = DateTime.Now;
        }

        public void Patch(RapidStreamingDto rapidStreamingData)
        {
            Netflix = OnApp(rapidStreamingData, "netflix");
            HBO = OnApp(rapidStreamingData, "hbo");
            Hulu = OnApp(rapidStreamingData, "hulu");
            DisneyPlus = OnApp(rapidStreamingData, "disney");
            AmazonPrime = OnApp(rapidStreamingData, "prime");
            ITunes = OnApp(rapidStreamingData, "itunes");
            GooglePlay = OnApp(rapidStreamingData, "google");
            LastUpdated = DateTime.Now;
        }

        public bool OnApp(RapidStreamingDto rapidStreamingData, string appName)
        {
            // If StreamingData is null, movie is not streaming anywhere.
            if (rapidStreamingData == null) { return false; }
            foreach(var location in rapidStreamingData.Locations)
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
