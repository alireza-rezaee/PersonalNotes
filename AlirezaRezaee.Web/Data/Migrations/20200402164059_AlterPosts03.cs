using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.Web.Data.Migrations
{
    public partial class AlterPosts03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summmary",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "Summmary",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Shares",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Articles",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ArticleSummaryArticleId",
                table: "ArticleCategory",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArticleSummary",
                columns: table => new
                {
                    ArticleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Summary = table.Column<string>(maxLength: 250, nullable: false),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    PublishDateTime = table.Column<DateTime>(nullable: false),
                    LatestUpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleSummary", x => x.ArticleId);
                });

            migrationBuilder.CreateTable(
                name: "ShareSummary",
                columns: table => new
                {
                    ShareId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    Summary = table.Column<string>(maxLength: 250, nullable: false),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    PublishDateTime = table.Column<DateTime>(nullable: false),
                    LatestUpdateDateTime = table.Column<DateTime>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareSummary", x => x.ShareId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategory_ArticleSummaryArticleId",
                table: "ArticleCategory",
                column: "ArticleSummaryArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleCategory_ArticleSummary_ArticleSummaryArticleId",
                table: "ArticleCategory",
                column: "ArticleSummaryArticleId",
                principalTable: "ArticleSummary",
                principalColumn: "ArticleId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleCategory_ArticleSummary_ArticleSummaryArticleId",
                table: "ArticleCategory");

            migrationBuilder.DropTable(
                name: "ArticleSummary");

            migrationBuilder.DropTable(
                name: "ShareSummary");

            migrationBuilder.DropIndex(
                name: "IX_ArticleCategory_ArticleSummaryArticleId",
                table: "ArticleCategory");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticleSummaryArticleId",
                table: "ArticleCategory");

            migrationBuilder.AddColumn<string>(
                name: "Summmary",
                table: "Shares",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summmary",
                table: "Articles",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }
    }
}
