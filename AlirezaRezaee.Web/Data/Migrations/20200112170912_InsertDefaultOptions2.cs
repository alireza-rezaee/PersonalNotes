using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.Web.Data.Migrations
{
    public partial class InsertDefaultOptions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'AboutAuthorSummary', N'اثر پذیرفته از انقلاب اسلامی ایران<br>مشغول به طراحی و توسعه نرم افزار<br>سرگرم به مطالعه، دانش افزایی و تعلیم آموزه ها<br>علاقه مند به فرهنگ، سیاست و تمدّن اسلامی<br>بنده ای از بندگان خدا، در تکاپوی حق')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Option] WHERE [OptionName] = N'AboutAuthorSummary'");
        }
    }
}
