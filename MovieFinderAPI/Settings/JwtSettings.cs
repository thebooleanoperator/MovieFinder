namespace MovieFinder.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public System.TimeSpan TokenLifetime { get; set; }
        public int RefreshLifetime { get; set; }
    }
}
