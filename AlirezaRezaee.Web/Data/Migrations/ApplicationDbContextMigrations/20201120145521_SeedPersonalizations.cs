using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class SeedPersonalizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO [Personalizations] ([Title], [Value])
            VALUES
            (N'SiteCoverSrc', N'/uploads/theme/cover.jpg'),
            (N'SiteLogoSrc', N'/uploads/theme/avatar-150x.jpg'),
            (N'TextLogoSrc', N'/uploads/theme/logo.png'),
            (N'IndexTitle', N'عنوان صفحه نخست سایت را اینجا بنویسید.'),
            (N'IndexDescription', N'توضیحات صفحه نخست سایت را اینجا بنویسید.'),
            (N'AdditionalTitle', N'عنوان کوتاه سایت را اینجا بنویسید.'),
            (N'SiteFootnote', N'" + "<div class=\"container\"><div class=\"row\"><div class=\"col mx-auto\"><div class=\"text-center small\">به این سایت خوش آمدید.</div></div></div><div class=\"row\"><div class=\"col\"><div class=\"text-center small text-secondary my-3\"><a class=\"text-secondary\" href=\"https://example.com/\"> Example.com </a> e.g. is licensed under a <a class=\"text-secondary\" href=\"https://creativecommons.org/licenses/by/4.0/\" target=\"_blank\" rel=\"noopener\"> Creative Commons Attribution 4.0 International License </a></div></div></div></div>" + "')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
