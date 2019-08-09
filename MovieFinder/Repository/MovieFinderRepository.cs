using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Repository
{
    public class MovieFinderRepository<TEntity> : IMovieFinderRepository<TEntity>
        where TEntity : class
    {
        protected readonly DbSet<TEntity> DbSet;

        protected MovieFinderRepository(DbContext context)
        {
            DbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public TEntity Get(int id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSet;
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}
