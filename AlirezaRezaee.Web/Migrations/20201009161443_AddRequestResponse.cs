using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rezaee.Alireza.Web.Migrations
{
    public partial class AddRequestResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestResponse",
                columns: table => new
                {
                    RequestId = table.Column<string>(maxLength: 50, nullable: false),
                    Method = table.Column<string>(maxLength: 7, nullable: true),
                    HasHttps = table.Column<bool>(nullable: true),
                    Path = table.Column<string>(maxLength: 2048, nullable: true),
                    QueryString = table.Column<string>(maxLength: 2048, nullable: true),
                    StatusCode = table.Column<int>(nullable: false),
                    IP = table.Column<string>(maxLength: 15, nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponse", x => x.RequestId);
                });

            migrationBuilder.CreateTable(
                name: "RequestResponseDetails",
                columns: table => new
                {
                    RequestId = table.Column<string>(nullable: false),
                    Protocol = table.Column<string>(maxLength: 10, nullable: true),
                    Host = table.Column<string>(maxLength: 2048, nullable: true),
                    Referrer = table.Column<string>(maxLength: 2048, nullable: true),
                    RequestHeaders = table.Column<string>(nullable: true),
                    ResponseHeaders = table.Column<string>(nullable: true),
                    RequestBody = table.Column<string>(nullable: true),
                    ResponseBody = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestResponseDetails", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_RequestResponseDetails_RequestResponse_RequestId",
                        column: x => x.RequestId,
                        principalTable: "RequestResponse",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestResponseDetails");

            migrationBuilder.DropTable(
                name: "RequestResponse");
        }
    }
}
