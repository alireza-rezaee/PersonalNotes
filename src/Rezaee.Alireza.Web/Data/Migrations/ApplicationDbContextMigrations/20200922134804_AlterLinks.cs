using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class AlterLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExpanded",
                table: "Links",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpanded",
                table: "Links");
        }
    }
}
