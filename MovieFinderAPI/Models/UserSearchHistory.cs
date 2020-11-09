namespace MovieFinder.Models
{
    public partial class UserSearchHistory
    {
        public int UserSearchHistoryId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public bool IsDeleted { get; set; }
    }
}
