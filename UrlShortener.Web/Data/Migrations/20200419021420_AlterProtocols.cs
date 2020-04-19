using Microsoft.EntityFrameworkCore.Migrations;

namespace UrlShortener.Web.Data.Migrations
{
    public partial class AlterProtocols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Protocols_Urls_UrlId",
                table: "Protocols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Protocols",
                table: "Protocols");

            migrationBuilder.DropColumn(
                name: "UrlId",
                table: "Protocols");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Protocols",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Protocols",
                table: "Protocols",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Urls_ProtocolId",
                table: "Urls",
                column: "ProtocolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_Protocols_ProtocolId",
                table: "Urls",
                column: "ProtocolId",
                principalTable: "Protocols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_Protocols_ProtocolId",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_Urls_ProtocolId",
                table: "Urls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Protocols",
                table: "Protocols");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Protocols");

            migrationBuilder.AddColumn<int>(
                name: "UrlId",
                table: "Protocols",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Protocols",
                table: "Protocols",
                column: "UrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_Protocols_Urls_UrlId",
                table: "Protocols",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
