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

        public IEnumerable<ImdbIds> GetByTitleAndYear(string title, int? year)
        {
            if (year != null && year > 0)
            {
                return DbSet.Where(i => i.Title.ToLower() == title.ToLower() && i.Year == year);
            }
            return DbSet.Where(i => i.Title.ToLower() == title.ToLower());
        }
    }
}
