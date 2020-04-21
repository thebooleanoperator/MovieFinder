namespace MovieFinder.Models
{
    public partial class RateLimits
    {
        public int RateLimitId { get; set; }
        public int RequestsRemaining { get; set; }
    }
}
