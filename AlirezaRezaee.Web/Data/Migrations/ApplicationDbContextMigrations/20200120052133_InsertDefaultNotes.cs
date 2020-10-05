using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class InsertDefaultNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO [Notes] ([DateTime], [Content]) VALUES('2020-01-13 15:30:00', N'آماده دریافت نقطه نظرات، انتقادات و پیشنهادات ثمر بخش همه شما عزیزان هستم.')
            INSERT INTO [Notes] ([DateTime], [Content]) VALUES('2020-01-09 00:00:00', N'نگارش اولیه این وبگاه پس از تکمیل فرایند طراحی و توسعه، راه اندازی و تا هفته های آتی آماده بهرهبرداری خواهد شد.')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Option]");
        }
    }
}
