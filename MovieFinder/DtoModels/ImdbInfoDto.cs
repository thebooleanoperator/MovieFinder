using System.Collections.Generic;

namespace MovieFinder.DtoModels
{
    public class ImdbInfoDto
    {
        public int MovieId { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public string RunTime { get; set; }
        public List<RatingsDto> Ratings { get; set; }
        public string ImdbId { get; set; }
        public string Plot { get; set; }
    }
}
