namespace MovieFinder.DtoModels
{
    public class AuthenticationDto
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public int UserId { get; set; }
    }
}
