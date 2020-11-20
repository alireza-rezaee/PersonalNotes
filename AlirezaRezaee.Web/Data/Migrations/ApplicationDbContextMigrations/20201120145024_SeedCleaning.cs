using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class SeedCleaning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [Posts]
                DBCC CHECKIDENT ('[Posts]', RESEED, 0)
                GO

                DELETE FROM [Tags]
                DBCC CHECKIDENT ('[Tags]', RESEED, 0)
                GO

                DELETE FROM [Blocks]
                DBCC CHECKIDENT ('[Blocks]', RESEED, 0)
                GO

                DELETE FROM [Messages]
                DBCC CHECKIDENT ('[Messages]', RESEED, 0)
                GO

                DELETE FROM [Pages]
                DBCC CHECKIDENT ('[Pages]', RESEED, 0)
                GO

                DELETE FROM [Personalizations]
                GO

                DELETE FROM [Pins]
                GO

                DELETE FROM [Posterpins]
                GO

                DELETE FROM [Links]
                GO

                DELETE FROM [Files]
                GO
            ");
        }
    }
}
