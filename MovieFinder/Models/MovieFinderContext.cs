using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;

namespace MovieFinder
{
    public class MovieFinderContext : DbContext
    {
        public MovieFinderContext()
        {
        }

        public MovieFinderContext(DbContextOptions<MovieFinderContext> options) : base(options)
        {

        }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<LikedMovies> LikedMovies { get; set;}
        public DbSet<Synopsis> Synopsis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Movies>(entity =>
                {
                    entity.HasKey(m => m.MovieId);
                
                    entity.Property(m => m.MovieId)
                          .HasColumnName("MovieId");

                    entity.Property(m => m.Year)
                          .HasColumnName("Year");

                    entity.Property(m => m.Director)
                          .HasColumnName("Director");

                    entity.Property(m => m.Title)
                          .HasColumnName("Title");

                    entity.Property(m => m.RunTime)
                          .HasColumnName("RunTime");

                    entity.Property(m => m.RottenTomatoesRating)
                          .HasColumnName("RottenTomatoesRating");

                    entity.Property(m => m.ImdbRating)
                          .HasColumnName("ImdbRating");

                    entity.Property(m => m.ImdbId)
                          .HasColumnName("ImdbId");

                    entity.Property(m => m.Poster)
                          .HasColumnName("Poster");
                });

            modelBuilder
                .Entity<Users>(entity =>
                {
                    entity.HasKey(m => m.UserId);

                    entity.Property(m => m.UserId)
                            .HasColumnName("UserId")
                            .IsRequired(); 

                    entity.Property(m => m.FirstName)
                            .HasColumnName("FirstName")
                            .IsRequired();

                    entity.Property(m => m.LastName)
                            .HasColumnName("LastName")
                            .IsRequired();

                    entity.Property(m => m.Email)
                            .HasColumnName("Email")
                            .IsRequired();

                    entity.Property(m => m.Password)
                            .HasColumnName("Password")
                            .IsRequired();
                });

            modelBuilder
                .Entity<LikedMovies>(entity =>
                {
                    entity.HasKey(m => m.LikedId);

                    entity.Property(m => m.LikedId)
                            .HasColumnName("LikedId");

                    entity.Property(m => m.UserId)
                            .HasColumnName("UserId");

                    entity.Property(m => m.MovieId)
                            .HasColumnName("MovieId");

                    entity.Property(m => m.DateCreated)
                            .HasColumnName("DateCreated")
                            .HasDefaultValueSql("GetUtcDate()"); 
                });

            modelBuilder
                .Entity<Synopsis>(entity =>
                {
                    entity.HasKey(m => m.SynopsisId);

                    entity.Property(m => m.SynopsisId)
                          .HasColumnName("SynopsisId");

                    entity.Property(m => m.Plot)
                          .HasColumnName("Plot");

                    entity.Property(m => m.MovieId)
                          .HasColumnName("MovieId");
                });

            modelBuilder
                .Entity<MovieTitles>(entity =>
                {
                    entity.HasKey(m => m.MovieTitleId);

                    entity.Property(m => m.MovieTitleId)
                            .HasColumnName("MovieTitleId");

                    entity.Property(m => m.MovieTitle)
                            .HasColumnName("MovieTitle");

                    entity.Property(m => m.Year)
                            .HasColumnName("Year"); 

                });

            modelBuilder
                .Entity<ImdbIds>(entity =>
                {
                    entity.HasKey(m => m.ImdbId);
                              
                    entity.Property(m => m.ImdbId)
                            .HasColumnName("ImdbId")
                            .ValueGeneratedNever();

                    entity.Property(m => m.Title)
                            .HasColumnName("Title");

                    entity.Property(m => m.Year)
                            .HasColumnName("Year"); 
                });

            modelBuilder
                .Entity<Genres>(entity =>
                {
                    entity.HasKey(m => m.GenreId);

                    entity.Property(m => m.GenreId)
                            .HasColumnName("GenreId");

                    entity.Property(m => m.Action)
                            .HasColumnName("Action");

                    entity.Property(m => m.Adventure)
                            .HasColumnName("Adventure");

                    entity.Property(m => m.Horror)
                            .HasColumnName("Horror");

                    entity.Property(m => m.Biography)
                            .HasColumnName("Biography");

                    entity.Property(m => m.Comedy)
                            .HasColumnName("Comedy");

                    entity.Property(m => m.Crime)
                            .HasColumnName("Crime");

                    entity.Property(m => m.Thriller)
                            .HasColumnName("Thriller");
                });
        }
    }
}
