using Microsoft.EntityFrameworkCore.Migrations;
using System.Web;

namespace AlirezaRezaee.Web.Data.Migrations
{
    public partial class InsertDefaultOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'FirstName', N'علیرضا')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'LastName', N'رضائی')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'AvatarOrginalPath', N'/uploads/profile/avatar-32x.jpg')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'AvatarPath_64px', N'/uploads/profile/avatar-64x.jpg')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'AvatarPath_100px', N'/uploads/profile/avatar-100x.jpg')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'AvatarPath_125px', N'/uploads/profile/avatar-125x.jpg')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'AvatarPath_150px', N'/uploads/profile/avatar-150x.jpg')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'CoverPath', N'/uploads/profile/cover.jpg')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'IllustratedNamePath', N'/uploads/profile/logo.png')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'SiteFootnote', N'" + HttpUtility.HtmlEncode("<div class=\"container\"><div class=\"row\"><div class=\"col mx-auto\"><div class=\"text-center small\">به تارنمای شخصی علیرضا رضائی خوش آمدید.<br>پیام ها، بازخورد ها و سفارشات خود را با من درمیان بگذارید.</div></div></div><div class=\"row\"><div class=\"col\"><div class=\"text-center small text-secondary my-3\"><a href=\"https://alireza.rezaee.org/\" class=\"text-secondary\">Alireza.Rezaee.org</a> is licensed under a <a href=\"https://creativecommons.org/licenses/by/4.0/\" target=\"_blank\" class=\"text-secondary\">Creative Commons Attribution 4.0 International License</a></div></div></div></div>") + "')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Option]");
        }
    }
}