using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.Web.Data.Migrations
{
    public partial class RemoveExtraTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "ArticleSummaryArticleId",
                table: "ArticleCategory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArticleSummaryArticleId",
                table: "ArticleCategory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArticleSummary",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LatestUpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UrlTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleSummary", x => x.ArticleId);
                });

            migrationBuilder.CreateTable(
                name: "ShareSummary",
                columns: table => new
                {
                    ShareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LatestUpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UrlTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
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
    }
}
