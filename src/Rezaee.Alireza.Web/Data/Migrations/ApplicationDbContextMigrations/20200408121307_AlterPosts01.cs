using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class AlterPosts01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Articles",
                maxLength: 150,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Posts",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
