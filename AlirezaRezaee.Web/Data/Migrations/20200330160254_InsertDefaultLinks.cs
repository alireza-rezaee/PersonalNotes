using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations
{
    public partial class InsertDefaultLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO[Links]([Rank], [Title], [Url], [ImagePath]) VALUES
                (1, N'دفتر حفظ و نشر آثار امام خمینی(ره)', N'http://www.imam-khomeini.ir/', N'/uploads/links/001-imam-khomeiniـir.jpg'),
                (2, N'دفتر حفظ و نشر آثار آیت الله خامنه‌ای(مد‌ظله‌العالی)', N'https://farsi.khamenei.ir/', N'/uploads/links/002-Khamenei.ir.jpg'),                
                (3, N'انتشارات انقلاب اسلامی', N'http://book-khamenei.ir/', N'/uploads/links/006-book-khamenei.ir.jpg'),                
                (4, N'پایگاه اطلاع رسانی ریاست جمهوری', N'http://president.ir/', N'/uploads/links/003-dolat.ir.jpg'),
                (5, N'مجلس شورای اسلامی', N'https://www.parliran.ir/', N'/uploads/links/004-parliran.ir.jpg'),
                (6, N'قوه قضاییه جمهوری اسلامی ایران', N'http://www.dadiran.ir/', N'/uploads/links/005-dadiran.ir.jpg')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Links]");
        }
    }
}
