﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class InsertDefaultOptions3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO[Options]([OptionName], [OptionValue]) VALUES(N'FullName', N'علیرضا رضائی')
            INSERT INTO[Options] ([OptionName], [OptionValue]) VALUES(N'IndexTitle', N'وبسایت علیرضا رضائی')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Option] WHERE [OptionName] = N'AboutAuthorSummary' AND [OptionName] = N'Title'");
        }
    }
}
