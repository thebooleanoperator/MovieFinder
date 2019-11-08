﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieFinder;

namespace MovieFinder.Migrations
{
    [DbContext(typeof(MovieFinderContext))]
    partial class MovieFinderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieFinder.Models.ImdbIds", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ImdbId")
                        .HasColumnName("ImdbId");

                    b.Property<string>("Title")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.ToTable("ImdbIds");
                });

            modelBuilder.Entity("MovieFinder.Models.LikedMovies", b =>
                {
                    b.Property<int>("LikedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("LikedId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("DateCreated")
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<int>("MovieId")
                        .HasColumnName("MovieId");

                    b.Property<int>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("LikedId");

                    b.ToTable("LikedMovies");
                });

            modelBuilder.Entity("MovieFinder.Models.MovieTitles", b =>
                {
                    b.Property<int>("MovieTitleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MovieTitleId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MovieTitle")
                        .HasColumnName("MovieTitle");

                    b.Property<int>("Year")
                        .HasColumnName("Year");

                    b.HasKey("MovieTitleId");

                    b.ToTable("MovieTitles");
                });

            modelBuilder.Entity("MovieFinder.Models.Movies", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("MovieId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Director")
                        .HasColumnName("Director");

                    b.Property<string>("Genre")
                        .HasColumnName("Genre");

                    b.Property<string>("ImdbId")
                        .HasColumnName("ImdbId");

                    b.Property<decimal>("ImdbRating")
                        .HasColumnName("ImdbRating");

                    b.Property<decimal>("RottenTomatoesRating")
                        .HasColumnName("RottenTomatoesRating");

                    b.Property<int>("RunTime")
                        .HasColumnName("RunTime");

                    b.Property<string>("Title")
                        .HasColumnName("Title");

                    b.Property<int>("Year")
                        .HasColumnName("Year");

                    b.HasKey("MovieId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieFinder.Models.Synopsis", b =>
                {
                    b.Property<int>("SynopsisId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("SynopsisId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MovieId")
                        .HasColumnName("MovieId");

                    b.Property<string>("SynopsisSummary")
                        .HasColumnName("SynopsisSummary");

                    b.HasKey("SynopsisId");

                    b.ToTable("Synopsis");
                });

            modelBuilder.Entity("MovieFinder.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("UserId")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("Email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("LastName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("Password");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
