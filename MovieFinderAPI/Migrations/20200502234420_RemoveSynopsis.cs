using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieFinder.Migrations
{
    public partial class RemoveSynopsis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Synopsis");

            migrationBuilder.AddColumn<string>(
                name: "Plot",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plot",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "Synopsis",
                columns: table => new
                {
                    SynopsisId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MovieId = table.Column<int>(nullable: false),
                    Plot = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synopsis", x => x.SynopsisId);
                });
        }
    }
}
