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

                    entity.Property(m => m.Genre)
                          .HasColumnName("Genre");

                    entity.Property(m => m.Year)
                          .HasColumnName("Year");

                    entity.Property(m => m.Director)
                          .HasColumnName("Director");

                    entity.Property(m => m.RunTime)
                          .HasColumnName("RunTime");
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

                    entity.Property(m => m.SynopsisSummary)
                          .HasColumnName("SynopsisSummary");

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
                   });
        }
    }
}
