namespace MovieFinder.Models
{
    public partial class Movies
    {
        public int MovieId { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public string RunTime { get; set; }
        public string RottenTomatoesRating { get; set; }
        public string ImdbRating { get; set; }
        public string ImdbId { get; set; }
        public string Poster { get; set; }
    }
}
