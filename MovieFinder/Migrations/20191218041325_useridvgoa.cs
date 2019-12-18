using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieFinder.Migrations
{
    public partial class useridvgoa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_UUID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UUID",
                table: "AspNetUsers",
                newName: "UserId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "AspNetUsers",
                newName: "UUID");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_UUID",
                table: "AspNetUsers",
                column: "UUID");
        }
    }
}
