using MovieFinder.Models;
using System.Collections.Generic;

namespace MovieFinder.Repository.Interface
{
    public interface IRefreshTokenRepository : IMovieFinderRepository<RefreshToken>
    {
        RefreshToken GetByToken(string guid);
        RefreshToken GetByUserId(int userId, bool includeInvalidated = false);
    }
}
