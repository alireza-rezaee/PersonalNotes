using Microsoft.EntityFrameworkCore.Migrations;

namespace UrlShortener.Web.Data.Migrations
{
    public partial class InsertDefaultProtocols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO[Protocols]([ProtocolName]) VALUES
                ('https'),
                ('http')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Protocols]");
        }
    }
}
