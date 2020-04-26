using MovieFinder.Enum;

namespace MovieFinder.Models
{
    public partial class RateLimits
    {
        public RateLimitsEnum RateLimitId { get; set; }
        public int RequestsRemaining { get; set; }
    }
}
