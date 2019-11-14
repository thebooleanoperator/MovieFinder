namespace MovieFinder.Models
{
    public partial class Genres
    {
        public int GenreId { get; set; }
        public int MovieId { get; set; }
        public bool Action { get; set; }
        public bool Adventure { get; set; }
        public bool Horror { get; set; }
        public bool Biography { get; set; }
        public bool Comedy { get; set; }
        public bool Crime { get; set; }
        public bool Thriller { get; set; }
        public bool Romance { get; set; }
    }
}
