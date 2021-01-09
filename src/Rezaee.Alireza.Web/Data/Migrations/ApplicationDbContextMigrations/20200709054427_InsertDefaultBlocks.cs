using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class InsertDefaultBlocks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\SqlQueries\InsertDefaultBlocks\InsertDefaults01.sql")));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM [Blocks] GO");
        }
    }
}
