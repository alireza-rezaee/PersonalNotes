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
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'SiteFootnote', N'" + HttpUtility.HtmlEncode("<div class=\"container\"><div class=\"row\"><div class=\"col mx-auto\"><div class=\"text-center small\">به تارنمای شخصی علیرضا رضائی خوش آمدید.<br>پیام ها، بازخورد ها و سفارشات خود را با من درمیان بگذارید.</div></div></div><div class=\"row\"><div class=\"col\"><div class=\"text-center small text-secondary my-3\"><a href=\"https://alireza.rezaee.org/\" class=\"text-secondary\">Alireza.Rezaee.org</a> is licensed under a <a href=\"https://creativecommons.org/licenses/by/4.0/\" target=\"_blank\" class=\"text-secondary\">Creative Commons Attribution 4.0 International License</a></div></div></div></div>") + @"')
            INSERT INTO [Options] ([OptionName], [OptionValue]) VALUES(N'QuranAyah', N'" + HttpUtility.HtmlEncode("<div class=\"d-flex rounded-right justify-content-center align-items-center mx-auto mx-sm-0 mb-3 mb-sm-0\" style=\"width: 100px;\"><div class=\"text-center\" style=\"max-width: 100%;\"><div class=\"quran-logo mx-auto\"></div><div class=\"badge badge-secondary\">الصف - آیه ۱۳</div></div></div><div class=\"mx-auto px-2\"><div class=\"text-quran\">وَأُخْرَى تُحِبُّونَهَا <b style=\"color: #008000;\">نَصْرٌ مِنَ اللَّهِ وَفَتْحٌ قَرِيبٌ </b>وَبَشِّرِ الْمُؤْمِنِينَ</div><div class=\"mt-3\">و [رحمتى] ديگر كه آن را دوست داريد <b style=\"color: #008000;\">يارى و پيروزى نزديكى از جانب خداست </b>و مؤمنان را [بدان] بشارت ده</div></div>") + "')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Option]");
        }
    }
}