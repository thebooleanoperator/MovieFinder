using System;

namespace MovieFinder.Models
{
    public partial class UserSearchHistory
    {
        public int UserSearchHistoryId { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
    }
}
