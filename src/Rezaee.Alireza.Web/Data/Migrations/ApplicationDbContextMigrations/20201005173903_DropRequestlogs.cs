using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlirezaRezaee.PersonalNotes.WeblogApp.Data.Migrations.ApplicationDbContextMigrations
{
    public partial class DropRequestlogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requestlogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requestlogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Protocol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueryString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Referrer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestBodyFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestHeadersFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseBodyFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseContentLength = table.Column<long>(type: "bigint", nullable: true),
                    ResponseHeadersFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scheme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusCode = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requestlogs", x => x.Id);
                });
        }
    }
}
