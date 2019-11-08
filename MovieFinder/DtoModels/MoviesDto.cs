namespace MovieFinder.Partialscs
{
    public class MoviesDto
    {
        public int MovieId { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public int RunTime { get; set; }
        public decimal ImdbRating { get; set; }
        public decimal RottenTomatoesRating { get; set; }
        public string ImdbId { get; set; }
    }
}
