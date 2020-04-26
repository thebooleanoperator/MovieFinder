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
        
        public ImdbIds Get(string imdbId)
        {
            return DbSet.Where(i => i.ImdbId == imdbId).SingleOrDefault();
        }

        public IEnumerable<ImdbIds> GetByTitle(string title, int? year, bool exactMatch = true)
        {
            // Only get titles with exact matching title. 
            if (exactMatch)
            {
                return year == null
                    ? DbSet.Where(i => i.Title.ToLower() == title.ToLower())
                    : DbSet.Where(i => i.Title.ToLower() == title.ToLower() && i.Year == year);
            }
            // Use contains to get all titles like title parameter. 
            else
            {
                return year == null
                    ? DbSet.Where(i => i.Title.ToLower().Contains(title.ToLower()))
                    : DbSet.Where(i => i.Title.ToLower().Contains(title.ToLower()) && i.Year == year);
            }
        }
    }
}
