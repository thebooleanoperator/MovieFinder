using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieFinder.Migrations
{
    public partial class GenreRomance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Romance",
                table: "Genres",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Romance",
                table: "Genres");
        }
    }
}
