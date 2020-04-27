using System;

namespace MovieFinder.Models
{
    public partial class StreamingData
    {
        public int StreamingDataId { get; set; }
        public int MovieId { get; set; }
        public string Netflix { get; set; }
        public string HBO { get; set; }
        public string Hulu { get; set; }
        public string DisneyPlus { get; set; }
        public string AmazonPrime { get; set; }
        public string ITunes { get; set; }
        public string GooglePlay { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

