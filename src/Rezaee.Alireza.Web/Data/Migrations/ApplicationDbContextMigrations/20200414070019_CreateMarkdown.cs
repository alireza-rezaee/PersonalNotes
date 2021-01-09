using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class CreateMarkdown : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Markdowns",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false),
                    UrlTitle = table.Column<string>(maxLength: 150, nullable: true),
                    FileUrl = table.Column<string>(maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markdowns", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Markdowns_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostSummaryViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    ThumbnailUrl = table.Column<string>(nullable: true),
                    PublishDateTime = table.Column<DateTime>(nullable: false),
                    LatestUpdateDateTime = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: true),
                    PostUrl = table.Column<string>(nullable: true),
                    PostEditUrl = table.Column<string>(nullable: true),
                    postDeleteUrl = table.Column<string>(nullable: true),
                    postEditTypeUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostSummaryViewModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Markdowns");

            migrationBuilder.DropTable(
                name: "PostSummaryViewModel");
        }
    }
}
