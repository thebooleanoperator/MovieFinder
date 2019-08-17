using Microsoft.EntityFrameworkCore;
using MovieFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieFinder
{
    public class MovieFinderContext : DbContext
    {
        public MovieFinderContext(DbContextOptions options) : base(options)
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
                         .HasColumnName("UserId");

                   entity.Property(m => m.FirstName)
                         .HasColumnName("FirstName");

                   entity.Property(m => m.LastName)
                         .HasColumnName("LastName"); 
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
        }
    }
}
