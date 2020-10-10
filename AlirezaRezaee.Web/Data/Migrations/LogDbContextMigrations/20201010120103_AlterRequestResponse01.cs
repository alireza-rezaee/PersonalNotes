using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations.LogDbContextMigrations
{
    public partial class AlterRequestResponse01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "RequestResponse",
                maxLength: 450,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RequestResponse");
        }
    }
}
