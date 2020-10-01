using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieFinder.Migrations
{
    public partial class titleposterlikedmovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Poster",
                table: "LikedMovies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LikedMovies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poster",
                table: "LikedMovies");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "LikedMovies");
        }
    }
}
