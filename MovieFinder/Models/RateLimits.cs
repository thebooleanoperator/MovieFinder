namespace MovieFinder.Models
{
    public class RateLimits
    {
        public int RateLimitId { get; set; }
        public int RequestsRemaining { get; set; }
    }
}
