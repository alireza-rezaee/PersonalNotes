using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class AddPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Html = table.Column<string>(nullable: true),
                    HasLayout = table.Column<bool>(nullable: false),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false),
                    IsVisible = table.Column<bool>(nullable: false),
                    ImageCoverPath = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pages");
        }
    }
}
