using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations
{
    public partial class AlterBlocks03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Rank",
                table: "Blocks",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "Position",
                table: "Blocks",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "Rank",
                table: "Blocks",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(byte),
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Position",
                table: "Blocks",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(byte));
        }
    }
}
