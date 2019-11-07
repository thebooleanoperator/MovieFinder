using Microsoft.EntityFrameworkCore;
using MovieFinder.Repository.Interface;
using MovieFinder.Repository.Repo;

namespace MovieFinder.Repository
{
    public class UnitOfWork
    {
        public IMoviesRepository Movies { get; set; }
        public ILikedMoviesRepository LikedMovies { get; set; }
        public ISynopsisRepository Synopsis { get; set; }
        public IUsersRepository Users { get; set; }
        public IMovieTitlesRepository MovieTitles { get; set; }
       
        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        
            Movies = new MoviesRepository(_context);
            LikedMovies = new LikedMoviesRepository(_context);
            Synopsis = new SynopsisRepository(_context);
            Users = new UsersRepository(_context);
            MovieTitles = new MovieTitlesRepository(_context);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges(); 
        }
    }
}
