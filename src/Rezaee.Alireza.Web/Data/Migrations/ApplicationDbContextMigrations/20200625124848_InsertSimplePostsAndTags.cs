using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class InsertSimplePostsAndTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //DELETE FROM [Articles] GO DELETE FROM [Shares] GO DELETE FROM [Posts] Go
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\SqlQueries\InsertSimplePostsAndTags\part1.sql")));
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\SqlQueries\InsertSimplePostsAndTags\part2.sql")));
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\SqlQueries\InsertSimplePostsAndTags\part3.sql")));
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\SqlQueries\InsertSimplePostsAndTags\part4.sql")));
            migrationBuilder.Sql(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\SqlQueries\InsertSimplePostsAndTags\part5.sql")));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM [Articles] GO DELETE FROM [Shares] GO DELETE FROM [Posts] Go DELETE FROM [PostTags] Go DELETE FROM [Tags] Go");
        }
    }
}
