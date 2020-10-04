using System;

namespace MovieFinder.Models
{
    public partial class StreamingData
    {
        public int StreamingDataId { get; set; }
        public bool Netflix { get; set; }
        public bool HBO { get; set; }
        public bool Hulu { get; set; }
        public bool DisneyPlus { get; set; }
        public bool AmazonPrime { get; set; }
        public bool ITunes { get; set; }
        public bool GooglePlay { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

