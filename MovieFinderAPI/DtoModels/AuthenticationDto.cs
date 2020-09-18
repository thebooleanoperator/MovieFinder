using Microsoft.IdentityModel.Tokens;

namespace MovieFinder.DtoModels
{
    public class AuthenticationDto
    {
        public string Token { get; set; }
        public string Error { get; set; }
        public bool IsSuccess { get; set; }
        public UsersDto UserDto { get; set; }
    }
}
