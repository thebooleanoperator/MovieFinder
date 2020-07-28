using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;

namespace MovieFinder
{
    public class MovieFinderContext : IdentityDbContext<Users>
    {
        public MovieFinderContext()
        {
        }

        public MovieFinderContext(DbContextOptions<MovieFinderContext> options) : base(options)
        {

        }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<LikedMovies> LikedMovies { get; set;}
        public DbSet<MovieTitles> MovieTitles { get; set; }
        public DbSet<ImdbIds> ImdbIds { get; set; }
        public DbSet<Genres> Genres { get; set; }
        public DbSet<StreamingData> StreamingData { get; set; }
        public DbSet<RateLimits> RateLimits { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
                .Entity<Users>(entity =>
                {
                    entity.HasKey(m => m.Id);

                    entity.HasAlternateKey(m => m.UserId);

                    entity.Property(m => m.UserId)
                            .ValueGeneratedOnAdd();

                    entity.Property(m => m.FirstName);

                    entity.Property(m => m.LastName);
                });

            modelBuilder
                .Entity<Movies>(entity =>
                {
                    entity.HasKey(m => m.MovieId);

                    entity.Property(m => m.MovieId);

                    entity.Property(m => m.Year);

                    entity.Property(m => m.Director);

                    entity.Property(m => m.Title);

                    entity.Property(m => m.RunTime);

                    entity.Property(m => m.RottenTomatoesRating);

                    entity.Property(m => m.ImdbRating);

                    entity.Property(m => m.ImdbId);

                    entity.Property(m => m.Plot);

                    entity.Property(m => m.Poster);

                    entity.Property(m => m.IsRec);
                });

            modelBuilder
                .Entity<LikedMovies>(entity =>
                {
                    entity.HasKey(m => m.LikedId);

                    entity.Property(m => m.LikedId);

                    entity.Property(m => m.UserId);

                    entity.Property(m => m.MovieId);
                });

            modelBuilder
                .Entity<MovieTitles>(entity =>
                {
                    entity.HasKey(m => m.MovieTitleId);

                    entity.Property(m => m.MovieTitleId);

                    entity.Property(m => m.MovieTitle);

                    entity.Property(m => m.Year);
                });

            modelBuilder
                .Entity<ImdbIds>(entity =>
                {
                    entity.HasKey(m => m.ImdbId);

                    entity.Property(m => m.ImdbId)
                            .ValueGeneratedNever();

                    entity.Property(m => m.Title);

                    entity.Property(m => m.Year);
                });

            modelBuilder
                .Entity<Genres>(entity =>
                {
                    entity.HasKey(m => m.GenreId);

                    entity.Property(m => m.GenreId);

                    entity.Property(m => m.Action);

                    entity.Property(m => m.Adventure);

                    entity.Property(m => m.Horror);

                    entity.Property(m => m.Biography);

                    entity.Property(m => m.Comedy);

                    entity.Property(m => m.Crime);

                    entity.Property(m => m.Thriller);

                    entity.Property(m => m.Romance);
                });

            modelBuilder
                .Entity<StreamingData>(entity =>
                {
                    entity.HasKey(m => m.StreamingDataId);

                    entity.Property(m => m.StreamingDataId);

                    entity.Property(m => m.Netflix);

                    entity.Property(m => m.HBO);

                    entity.Property(m => m.Hulu);

                    entity.Property(m => m.DisneyPlus);

                    entity.Property(m => m.AmazonPrime);

                    entity.Property(m => m.ITunes);

                    entity.Property(m => m.GooglePlay);

                    entity.Property(m => m.LastUpdated);
                });

            modelBuilder
                .Entity<RateLimits>(entity =>
                {
                    entity.HasKey(m => m.RateLimitId);

                    entity.Property(m => m.RateLimitId)
                        .ValueGeneratedNever();

                    entity.Property(m => m.RequestsRemaining);
                });

            modelBuilder
                .Entity<RefreshToken>(entity =>
                {
                    entity.HasKey(m => m.Token);

                    entity.Property(m => m.Token)
                        .ValueGeneratedNever();

                    entity.Property(m => m.JwtId);

                    entity.Property(m => m.ExpirationDate);

                    entity.Property(m => m.IsUsed);

                    entity.Property(m => m.Invalidated);

                    entity.Property(m => m.UserId);
                });

            modelBuilder
                 .Entity<UserSearchHistory>(entity =>
                 {
                     entity.HasKey(m => m.UserSearchHistoryId);

                     entity.Property(m => m.UserSearchHistoryId);

                     entity.Property(m => m.MovieId);

                     entity.Property(m => m.UserId);

                     entity.Property(m => m.Title);

                     entity.Property(m => m.Poster);
                 });
        }
    }
}
