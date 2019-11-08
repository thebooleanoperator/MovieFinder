using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieFinder.Migrations
{
    public partial class AddingImdbIds2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImdbId",
                table: "Movies",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ImdbRating",
                table: "Movies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RottenTomatoesRating",
                table: "Movies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "ImdbId",
                table: "ImdbIds",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImdbId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ImdbRating",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "RottenTomatoesRating",
                table: "Movies");

            migrationBuilder.AlterColumn<int>(
                name: "ImdbId",
                table: "ImdbIds",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
