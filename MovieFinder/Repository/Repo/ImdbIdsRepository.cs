using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using MovieFinder.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace MovieFinder.Repository.Repo
{
    public class ImdbIdsRepository : MovieFinderRepository<ImdbIds>, IImdbIdsRepository 
    {
        public ImdbIdsRepository(DbContext context) : base(context)
        {

        }
        
        public ImdbIds GetByImdbId(string imdbId)
        {
            return DbSet.Where(i => i.ImdbId == imdbId).SingleOrDefault();
        }

        public IEnumerable<ImdbIds> GetByTitle(string title)
        {
            title = title.Replace(" ", "").ToLower();
            return DbSet.Where(i => i.Title.Replace(" ", "").ToLower().Contains(title));
        }
    }
}
