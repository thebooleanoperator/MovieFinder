using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieFinder.Migrations
{
    public partial class LikedMoviesRebuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_LikedMovies_MovieId",
                table: "LikedMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LikedMovies",
                table: "LikedMovies");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "RunTime",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "LikedMovies",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "LikedId",
                table: "LikedMovies",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikedMovies",
                table: "LikedMovies",
                column: "LikedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LikedMovies",
                table: "LikedMovies");

            migrationBuilder.DropColumn(
                name: "LikedId",
                table: "LikedMovies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Year",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "RunTime",
                table: "Movies",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "LikedMovies",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_LikedMovies_MovieId",
                table: "LikedMovies",
                column: "MovieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LikedMovies",
                table: "LikedMovies",
                column: "UserId");
        }
    }
}
