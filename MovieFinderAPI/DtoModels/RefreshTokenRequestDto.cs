namespace MovieFinder.DtoModels
{
    public class RefreshTokenRequestDto
    {
        public string JwtToken { get; set; }
        public int UserId { get; set; }
    }
}