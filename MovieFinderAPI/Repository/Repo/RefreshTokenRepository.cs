using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class RefreshTokenRepository : MovieFinderRepository<RefreshToken>, IRefreshTokenRepository 
    {
        public RefreshTokenRepository(DbContext context) : base (context)
        {

        }

        public RefreshToken GetByToken(string guid)
        {
            return DbSet.Where(r => r.Token == guid).SingleOrDefault(); 
        }
    }
}
