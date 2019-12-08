namespace MovieFinder.DtoModels
{
    public class MoviesDto
    {
        public int MovieId { get; set; }
        public string Genre { get; set; }
        public int Year {get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public string RunTime { get; set; }
        public decimal ImdbRating { get; set; }
        public decimal RottenTomatoesRating { get; set; }
        public string ImdbId { get; set; }
        public int? NetflixId { get; set; }
        public bool IsRec { get; set; }
    }
}
