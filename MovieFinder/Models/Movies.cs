namespace MovieFinder.Models
{
    public partial class Movies
    {
        public int MovieId { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public int? RunTime { get; set; }
        public int? RottenTomatoesRating { get; set; }
        public decimal? ImdbRating { get; set; }
        public string ImdbId { get; set; }
        public string Poster { get; set; }
        public int? NetflixId { get; set; }
        public bool IsRec { get; set; }
    }
}
