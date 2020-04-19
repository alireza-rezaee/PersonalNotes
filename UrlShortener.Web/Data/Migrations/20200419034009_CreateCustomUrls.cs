using Microsoft.EntityFrameworkCore.Migrations;

namespace UrlShortener.Web.Data.Migrations
{
    public partial class CreateCustomUrls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomUrls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(nullable: true),
                    CostomShortLink = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    ProtocolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomUrls_Protocols_ProtocolId",
                        column: x => x.ProtocolId,
                        principalTable: "Protocols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomUrls_ProtocolId",
                table: "CustomUrls",
                column: "ProtocolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomUrls");
        }
    }
}
