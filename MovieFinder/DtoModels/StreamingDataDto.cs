using System.Collections.Generic;

namespace MovieFinder.DtoModels
{
    public class StreamingDataDto
    {
        public string Netflix { get; set; }
        public string HBO { get; set; }
        public string Hulu { get; set; }
        public string DisneyPlus { get; set; }
        public string AmazonPrime { get; set; }
        public string ITunes { get; set; }
        public string GooglePlay { get; set; }
        public string Name { get; set; }
        public List<StreamingLocationsDto> Locations { get; set; }
    }
}
