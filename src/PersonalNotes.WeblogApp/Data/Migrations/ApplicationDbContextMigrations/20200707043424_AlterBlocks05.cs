using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class AlterBlocks05 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Scripts",
                table: "Blocks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Styles",
                table: "Blocks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Scripts",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "Styles",
                table: "Blocks");
        }
    }
}
