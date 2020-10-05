using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Data.Migrations
{
    public partial class AddRequestlogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requestlogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<string>(nullable: true),
                    Method = table.Column<string>(nullable: true),
                    Scheme = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    QueryString = table.Column<string>(nullable: true),
                    StatusCode = table.Column<int>(nullable: false),
                    Protocol = table.Column<string>(nullable: true),
                    ResponseContentLength = table.Column<long>(nullable: true),
                    Referrer = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    RequestBodyFilePath = table.Column<string>(nullable: true),
                    RequestHeadersFilePath = table.Column<string>(nullable: true),
                    ResponseBodyFilePath = table.Column<string>(nullable: true),
                    ResponseHeadersFilePath = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requestlogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requestlogs");
        }
    }
}
