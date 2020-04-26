using System.Collections.Generic;

namespace MovieFinder.DtoModels
{
    public class RapidStreamingDto
    {
        public string Name { get; set; }
        public int RequestsRemaining { get; set; }
        public List<StreamingDataDto> Locations { get; set; }
        public StreamingDataDto External_Ids {get;set;}
    }
}
