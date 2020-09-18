using System;

namespace MovieFinder.Models
{
    public partial class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int UserId { get; set; }
        public bool Invalidated { get; set; }
    }
}
