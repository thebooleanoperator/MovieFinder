namespace MovieFinder.Models
{
    public partial class LikedMovies
    {
        public LikedMovies()
        {

        }

        public int LikedId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
