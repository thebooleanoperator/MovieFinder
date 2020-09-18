using MovieFinder.DtoModels;
using MovieFinder.Models;
using System.Threading.Tasks;

namespace MovieFinder.Services.Interface
{
    public interface IIdentityService
    {
        /// <summary>
        /// Create a new user and return Dto with isSucess back to controller.
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        Task<RegisterDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthenticationDto> LoginAsync(LoginDto loginDto);
        AuthenticationDto GuestLogin();
        Task<bool> UpdatePassword(UpdatePasswordDto updatePasswordDto);
        string CreateJwtToken(Users user);
        RefreshToken CreateRefreshToken(string jwtToken, int userId);
        bool RefreshTokenIsValid(string jwtToken, string refreshToken);
    }
}
