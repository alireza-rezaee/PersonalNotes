using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Migrations
{
    public partial class AlterRequestResponse01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponseTime",
                table: "RequestResponse",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseTime",
                table: "RequestResponse");
        }
    }
}
