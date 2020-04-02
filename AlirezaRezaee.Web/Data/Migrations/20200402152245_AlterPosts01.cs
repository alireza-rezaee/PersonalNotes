using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.Web.Data.Migrations
{
    public partial class AlterPosts01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleCategory_ACategories_CategoryId",
                table: "ArticleCategory");

            migrationBuilder.DropTable(
                name: "ShareCategory");

            migrationBuilder.DropTable(
                name: "ACategories");

            migrationBuilder.DropTable(
                name: "SCategories");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "Summmary",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Summmary",
                table: "Articles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LatestUpdateDateTime",
                table: "Shares",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Shares",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Shares",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Shares",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LatestUpdateDateTime",
                table: "Articles",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Articles",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Articles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlTitle",
                table: "Articles",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleCategory_Categories_CategoryId",
                table: "ArticleCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleCategory_Categories_CategoryId",
                table: "ArticleCategory");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "CoverUrl",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "UrlTitle",
                table: "Articles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LatestUpdateDateTime",
                table: "Shares",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Shares",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summmary",
                table: "Shares",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LatestUpdateDateTime",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summmary",
                table: "Articles",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ACategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShareCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SCategoryId = table.Column<int>(type: "int", nullable: true),
                    ShareId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShareCategory_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShareCategory_ACategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ACategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShareCategory_SCategories_SCategoryId",
                        column: x => x.SCategoryId,
                        principalTable: "SCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareCategory_Shares_ShareId",
                        column: x => x.ShareId,
                        principalTable: "Shares",
                        principalColumn: "ShareId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareCategory_ArticleId",
                table: "ShareCategory",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareCategory_CategoryId",
                table: "ShareCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareCategory_SCategoryId",
                table: "ShareCategory",
                column: "SCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareCategory_ShareId",
                table: "ShareCategory",
                column: "ShareId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleCategory_ACategories_CategoryId",
                table: "ArticleCategory",
                column: "CategoryId",
                principalTable: "ACategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
