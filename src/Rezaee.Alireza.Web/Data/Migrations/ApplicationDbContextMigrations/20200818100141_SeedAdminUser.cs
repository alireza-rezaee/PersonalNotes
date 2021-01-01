using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class SeedAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Add Developer User Account (Alireza-rezaee)
            //Username: alireza@rezaee.org
            //Password: <Password123#
            migrationBuilder.Sql(@"INSERT INTO [AspNetUsers] (
            [Id],
            [UserName],
            [NormalizedUserName],
            [Email],
            [NormalizedEmail],
            [EmailConfirmed],
            [PasswordHash],
            [SecurityStamp],
            [ConcurrencyStamp],
            [PhoneNumber],
            [PhoneNumberConfirmed],
            [TwoFactorEnabled],
            [LockoutEnd],
            [LockoutEnabled],
            [AccessFailedCount],
            [BirthDate],
            [DisplayName],
            [FirstName],
            [LastName],
            [ProfileImagePath],
            [RegisteredDateTime],
            [Location])

            VALUES (
            N'c3ad1d11-31fc-4079-94d4-e74ab1273112',
            N'alireza@rezaee.org',
            N'ALIREZA@REZAEE.ORG',
            N'alireza@rezaee.org',
            N'ALIREZA@REZAEE.ORG',
            1,
            N'AQAAAAEAACcQAAAAELQst08RtM8VXNlWF2BgYvp1rzx0CAIuq8jRqejEW0kUO9kEXOBwb3Zw9l63iU3MGg==',
            N'2TFME42VSRCHGH5Y3PZO6TYEUV6HQJFU',
            N'8ea91422-a994-4cb9-b0e1-0ee117accd97',
            NULL,
            0,
            0,
            NULL,
            1,
            0,
            N'1999-03-08 00:00:00',
            N'علیرضا رضائی',
            N'علی‌رضا',
            N'رضائی',
            NULL,
            N'2020-08-18 14:35:14',
            N'ایران، تهران')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM [AspNetUsers] WHERE Id = N'c3ad1d11-31fc-4079-94d4-e74ab1273112'");
        }
    }
}
