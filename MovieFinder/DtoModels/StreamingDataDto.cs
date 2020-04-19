namespace MovieFinder.DtoModels
{
    public class StreamingDataDto
    {
        public string Display_Name { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public RapidImdbDto Imdb { get; set; }
    }
}
