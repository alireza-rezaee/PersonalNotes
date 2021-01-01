using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class InsertDefaultOptions5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO[Options]([OptionName], [OptionValue]) VALUES(N'NumberOfPrimaryLinks', '6')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Option] WHERE [OptionName] = N'NumberOfPrimaryLinks'");
        }
    }
}
