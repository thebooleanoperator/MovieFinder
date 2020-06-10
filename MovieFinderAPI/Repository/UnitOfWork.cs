using Microsoft.EntityFrameworkCore;
using MovieFinder.Repository.Interface;
using MovieFinder.Repository.Repo;
using System.Threading.Tasks;

namespace MovieFinder.Repository
{
    public class UnitOfWork
    {
        public IMoviesRepository Movies { get; set; }
        public ILikedMoviesRepository LikedMovies { get; set; }
        public IUsersRepository Users { get; set; }
        public IMovieTitlesRepository MovieTitles { get; set; }
        public IImdbIdsRepository ImdbIds { get; set; }
        public IGenresRepository Genres { get; set; }
        public IStreamingDataRepository StreamingData { get; set; }
        public IRateLimitsRepository RateLimits { get; set; }
        public IRefreshTokenRepository RefreshToken { get; set; }
        public IUserSearchHistoryRepository UserSearchHistory { get; set; }

        private readonly DbContext _context;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        
            Movies = new MoviesRepository(_context);
            LikedMovies = new LikedMoviesRepository(_context);
            Users = new UsersRepository(_context);
            MovieTitles = new MovieTitlesRepository(_context);
            ImdbIds = new ImdbIdsRepository(_context);
            Genres = new GenresRepository(_context);
            StreamingData = new StreamingDataRepository(_context);
            RateLimits = new RateLimitsRepository(_context);
            RefreshToken = new RefreshTokenRepository(_context);
            UserSearchHistory = new UserSearchHistoryRepository(_context);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges(); 
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync(); 
        }
    }
}
