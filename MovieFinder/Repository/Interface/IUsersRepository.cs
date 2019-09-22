using MovieFinder.Models;

namespace MovieFinder.Repository.Interface
{
    public interface IUsersRepository : IMovieFinderRepository<Users>
    {
        Users GetByEmail(string email);
    }
}
