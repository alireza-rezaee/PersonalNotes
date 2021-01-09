using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class CreatePostsArticlesShares : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 150, nullable: false),
                    UrlTitle = table.Column<string>(maxLength: 150, nullable: true),
                    Summary = table.Column<string>(maxLength: 250, nullable: false),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    PublishDateTime = table.Column<DateTime>(nullable: false),
                    LatestUpdateDateTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    HtmlContent = table.Column<string>(nullable: true),
                    SourcesUrl = table.Column<string>(nullable: true),
                    CoverUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Articles_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    RedirectToUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Shares_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
