using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class ImdbIdsRepository : MovieFinderRepository<ImdbIds>, IImdbIdsRepository 
    {
        public ImdbIdsRepository(DbContext context) : base(context)
        {

        }
        
        public ImdbIds GetByString(string imdbId)
        {
            var imdbIds = DbSet.Where(i => i.ImdbId == imdbId).Single();
            return imdbIds;
        }
    }
}
