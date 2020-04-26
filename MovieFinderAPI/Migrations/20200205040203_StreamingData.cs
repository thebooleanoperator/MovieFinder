using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieFinder.Migrations
{
    public partial class StreamingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreamingData",
                columns: table => new
                {
                    StreamingDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MovieId = table.Column<int>(nullable: false),
                    Netflix = table.Column<bool>(nullable: false),
                    HBO = table.Column<bool>(nullable: false),
                    Hulu = table.Column<bool>(nullable: false),
                    DisneyPlus = table.Column<bool>(nullable: false),
                    AmazonPrime = table.Column<bool>(nullable: false),
                    ITunes = table.Column<bool>(nullable: false),
                    GooglePlay = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamingData", x => x.StreamingDataId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreamingData");
        }
    }
}
