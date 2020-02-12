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
        public IImdbIdsRepository ImdbIds { get; set; }
        public IGenresRepository Genres { get; set; }
        public IStreamingDataRepository StreamingData { get; set; }

        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        
            Movies = new MoviesRepository(_context);
            LikedMovies = new LikedMoviesRepository(_context);
            Synopsis = new SynopsisRepository(_context);
            Users = new UsersRepository(_context);
            MovieTitles = new MovieTitlesRepository(_context);
            ImdbIds = new ImdbIdsRepository(_context);
            Genres = new GenresRepository(_context);
            StreamingData = new StreamingDataRepository(_context);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges(); 
        }
    }
}
