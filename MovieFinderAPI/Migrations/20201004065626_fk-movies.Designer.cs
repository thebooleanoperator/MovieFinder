﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieFinder;

namespace MovieFinder.Migrations
{
    [DbContext(typeof(MovieFinderContext))]
    [Migration("20201004065626_fk-movies")]
    partial class fkmovies
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MovieFinder.Models.Genres", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Action");

                    b.Property<bool>("Adventure");

                    b.Property<bool>("Biography");

                    b.Property<bool>("Comedy");

                    b.Property<bool>("Crime");

                    b.Property<bool>("Horror");

                    b.Property<int>("MovieId");

                    b.Property<bool>("Romance");

                    b.Property<bool>("Thriller");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MovieFinder.Models.ImdbIds", b =>
                {
                    b.Property<string>("ImdbId");

                    b.Property<string>("Title");

                    b.Property<int>("Year");

                    b.HasKey("ImdbId");

                    b.ToTable("ImdbIds");
                });

            modelBuilder.Entity("MovieFinder.Models.LikedMovies", b =>
                {
                    b.Property<int>("LikedId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("MovieId");

                    b.Property<string>("Poster");

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("LikedId");

                    b.ToTable("LikedMovies");
                });

            modelBuilder.Entity("MovieFinder.Models.Movies", b =>
                {
                    b.Property<int>("MovieId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Director");

                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<int?>("GenreId1");

                    b.Property<string>("ImdbId");

                    b.Property<decimal?>("ImdbRating");

                    b.Property<bool>("IsRec");

                    b.Property<string>("Plot");

                    b.Property<string>("Poster");

                    b.Property<int?>("RottenTomatoesRating");

                    b.Property<int?>("RunTime");

                    b.Property<int>("StreamingDataId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("Title");

                    b.Property<int>("Year");

                    b.HasKey("MovieId");

                    b.HasIndex("GenreId1");

                    b.HasIndex("StreamingDataId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MovieFinder.Models.MovieTitles", b =>
                {
                    b.Property<int>("MovieTitleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MovieTitle");

                    b.Property<int>("Year");

                    b.HasKey("MovieTitleId");

                    b.ToTable("MovieTitles");
                });

            modelBuilder.Entity("MovieFinder.Models.RateLimits", b =>
                {
                    b.Property<int>("RateLimitId");

                    b.Property<int>("RequestsRemaining");

                    b.HasKey("RateLimitId");

                    b.ToTable("RateLimits");
                });

            modelBuilder.Entity("MovieFinder.Models.RefreshToken", b =>
                {
                    b.Property<string>("Token");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("ExpirationDate");

                    b.Property<bool>("Invalidated");

                    b.Property<int>("UserId");

                    b.HasKey("Token");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("MovieFinder.Models.StreamingData", b =>
                {
                    b.Property<int>("StreamingDataId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AmazonPrime");

                    b.Property<bool>("DisneyPlus");

                    b.Property<bool>("GooglePlay");

                    b.Property<bool>("HBO");

                    b.Property<bool>("Hulu");

                    b.Property<bool>("ITunes");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<int>("MovieId");

                    b.Property<bool>("Netflix");

                    b.HasKey("StreamingDataId");

                    b.ToTable("StreamingData");
                });

            modelBuilder.Entity("MovieFinder.Models.Users", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasAlternateKey("UserId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MovieFinder.Models.UserSearchHistory", b =>
                {
                    b.Property<int>("UserSearchHistoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("MovieId");

                    b.Property<string>("Poster");

                    b.Property<string>("Title");

                    b.Property<int>("UserId");

                    b.HasKey("UserSearchHistoryId");

                    b.ToTable("UserSearchHistory");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MovieFinder.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MovieFinder.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MovieFinder.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MovieFinder.Models.Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MovieFinder.Models.Movies", b =>
                {
                    b.HasOne("MovieFinder.Models.Genres", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId1");

                    b.HasOne("MovieFinder.Models.StreamingData", "StreamingData")
                        .WithMany()
                        .HasForeignKey("StreamingDataId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
