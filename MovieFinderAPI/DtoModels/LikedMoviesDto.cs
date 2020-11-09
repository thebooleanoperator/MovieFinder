namespace MovieFinder.DtoModels
{
    public class LikedMoviesDto
    {
        public int LikedId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
    }
}
