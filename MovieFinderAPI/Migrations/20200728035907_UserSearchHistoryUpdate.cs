using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieFinder.Migrations
{
    public partial class UserSearchHistoryUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Poster",
                table: "UserSearchHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "UserSearchHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Poster",
                table: "UserSearchHistory");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "UserSearchHistory");
        }
    }
}
