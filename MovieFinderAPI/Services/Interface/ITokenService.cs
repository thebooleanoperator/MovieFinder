using MovieFinder.Models;

namespace MovieFinder.Services.Interface
{
    public interface ITokenService
    {
        string CreateJwtToken(Users user);
        /// <summary>
        /// Validate jwtToken is a valid token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool JwtTokenIsValid(string token);
        RefreshToken CreateRefreshToken(string jwtToken, int userId);
        bool RefreshTokenIsValid(RefreshToken refreshToken);
    }
}
