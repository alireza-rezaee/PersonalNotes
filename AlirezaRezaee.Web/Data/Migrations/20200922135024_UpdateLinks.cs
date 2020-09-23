using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations
{
    public partial class UpdateLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE [Links] SET [IsExpanded] = 1 WHERE [Rank] = 1;
                UPDATE [Links] SET [IsExpanded] = 1 WHERE [Rank] = 2;
                UPDATE [Links] SET [IsExpanded] = 1 WHERE [Rank] = 3;
                UPDATE [Links] SET [IsExpanded] = 1 WHERE [Rank] = 4;
                UPDATE [Links] SET [IsExpanded] = 1 WHERE [Rank] = 5;
                UPDATE [Links] SET [IsExpanded] = 1 WHERE [Rank] = 6;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE [Links] SET [IsExpanded] = 0 WHERE [Rank] = 1;
                UPDATE [Links] SET [IsExpanded] = 0 WHERE [Rank] = 2;
                UPDATE [Links] SET [IsExpanded] = 0 WHERE [Rank] = 3;
                UPDATE [Links] SET [IsExpanded] = 0 WHERE [Rank] = 4;
                UPDATE [Links] SET [IsExpanded] = 0 WHERE [Rank] = 5;
                UPDATE [Links] SET [IsExpanded] = 0 WHERE [Rank] = 6;");
        }
    }
}
