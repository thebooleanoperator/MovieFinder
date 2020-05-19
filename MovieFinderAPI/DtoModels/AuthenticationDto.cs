namespace MovieFinder.DtoModels
{
    public class AuthenticationDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Error { get; set; }
        public UsersDto UserDto { get; set; }
    }
}
