using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class UsersRepository : MovieFinderRepository<Users>, IUsersRepository 
    {
        public UsersRepository(DbContext context) : base(context)
        {

        }

        public Users GetByEmail(string email)
        {
            return DbSet.Where(e => e.Email == email).First();
        }
    }
}
