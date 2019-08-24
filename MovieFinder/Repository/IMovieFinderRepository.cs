using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder.Repository
{
    public interface IMovieFinderRepository<TEntity>
        where TEntity : class 
    {
        TEntity Get(int id);
        IQueryable<TEntity> GetAll(); 
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity); 
    }
}
