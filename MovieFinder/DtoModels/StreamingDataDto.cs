using System.Collections.Generic;

namespace MovieFinder.DtoModels
{
    public class StreamingDataDto
    {
        public bool Netflix { get; set; }
        public bool HBO { get; set; }
        public bool Hulu { get; set; }
        public bool DisneyPlus { get; set; }
        public bool AmazonPrime { get; set; }
        public string Name { get; set; }
        public List<StreamingLocationsDto> Locations { get; set; }
    }
}
