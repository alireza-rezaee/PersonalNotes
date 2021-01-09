using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class AlterPins03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pins",
                table: "Pins");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Pins");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Pins",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pins",
                table: "Pins",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pins_PostId",
                table: "Pins",
                column: "PostId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pins",
                table: "Pins");

            migrationBuilder.DropIndex(
                name: "IX_Pins_PostId",
                table: "Pins");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Pins");

            migrationBuilder.AddColumn<short>(
                name: "Rank",
                table: "Pins",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pins",
                table: "Pins",
                column: "PostId");
        }
    }
}
