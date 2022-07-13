using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class LogLogic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogType",
                columns: table => new
                {
                    Log_typeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Log_typeDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Log_typeCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogType", x => x.Log_typeID);
                });

            migrationBuilder.CreateTable(
                name: "LogEvent",
                columns: table => new
                {
                    Log_eventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Log_eventDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Log_eventCode = table.Column<int>(type: "int", nullable: false),
                    Log_typeID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEvent", x => x.Log_eventID);
                    table.ForeignKey(
                        name: "FK_LogEvent_LogType_Log_typeID",
                        column: x => x.Log_typeID,
                        principalTable: "LogType",
                        principalColumn: "Log_typeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogDetails = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Log_eventID = table.Column<int>(type: "int", nullable: false),
                    LogNotes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    LogBrowser = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LogBrowserVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LogIPAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogOS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LogOSVersion = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LogEventLog_eventID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_Log_LogEvent_LogEventLog_eventID",
                        column: x => x.LogEventLog_eventID,
                        principalTable: "LogEvent",
                        principalColumn: "Log_eventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Log_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Log_LogEventLog_eventID",
                table: "Log",
                column: "LogEventLog_eventID");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UserID",
                table: "Log",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogEvent_Log_typeID",
                table: "LogEvent",
                column: "Log_typeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "LogEvent");

            migrationBuilder.DropTable(
                name: "LogType");
        }
    }
}
