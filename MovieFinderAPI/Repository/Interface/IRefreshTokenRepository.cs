using MovieFinder.Models;

namespace MovieFinder.Repository.Interface
{
    public interface IRefreshTokenRepository : IMovieFinderRepository<RefreshToken>
    {
        RefreshToken GetByToken(string guid);
    }
}
