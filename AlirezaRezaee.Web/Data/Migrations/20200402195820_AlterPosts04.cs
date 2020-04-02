using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.Web.Data.Migrations
{
    public partial class AlterPosts04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "ShareSummary",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Shares",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "ArticleSummary",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Articles",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "ShareSummary");

            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "ArticleSummary");

            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Articles");
        }
    }
}
