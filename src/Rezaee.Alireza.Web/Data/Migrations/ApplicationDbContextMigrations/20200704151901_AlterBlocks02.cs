using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class AlterBlocks02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Rank",
                table: "Blocks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Blocks");
        }
    }
}
