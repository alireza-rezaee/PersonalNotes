using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations
{
    public partial class AddLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinksModel",
                columns: table => new
                {
                    Rank = table.Column<short>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Url = table.Column<string>(maxLength: 200, nullable: true),
                    ImagePath = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinksModel", x => x.Rank);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinksModel");
        }
    }
}
