using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class Add_Geocalocation_And_Typology : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LatitudeUser",
                table: "User",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "LongitudeUser",
                table: "User",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidenceGeolocationId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Geolocation",
                columns: table => new
                {
                    GeolocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geolocation", x => x.GeolocationId);
                });

            migrationBuilder.CreateTable(
                name: "Typology",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Typology", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttributesGeolocation",
                columns: table => new
                {
                    AttributesGeolocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeolocationId = table.Column<int>(type: "int", nullable: false),
                    AttributesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributesGeolocation", x => x.AttributesGeolocationId);
                    table.ForeignKey(
                        name: "FK_AttributesGeolocation_Attributes_AttributesId",
                        column: x => x.AttributesId,
                        principalTable: "Attributes",
                        principalColumn: "AttributesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttributesGeolocation_Geolocation_GeolocationId",
                        column: x => x.GeolocationId,
                        principalTable: "Geolocation",
                        principalColumn: "GeolocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGeolocation",
                columns: table => new
                {
                    UserGeolocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserdId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GeolocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGeolocation", x => x.UserGeolocationId);
                    table.ForeignKey(
                        name: "FK_UserGeolocation_Geolocation_GeolocationId",
                        column: x => x.GeolocationId,
                        principalTable: "Geolocation",
                        principalColumn: "GeolocationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGeolocation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AttributesTypology",
                columns: table => new
                {
                    AttributesTypologyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributesId = table.Column<int>(type: "int", nullable: false),
                    TypologyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributesTypology", x => x.AttributesTypologyId);
                    table.ForeignKey(
                        name: "FK_AttributesTypology_Attributes_AttributesId",
                        column: x => x.AttributesId,
                        principalTable: "Attributes",
                        principalColumn: "AttributesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttributesTypology_Typology_TypologyId",
                        column: x => x.TypologyId,
                        principalTable: "Typology",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTypology",
                columns: table => new
                {
                    UserTypologyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TypologyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTypology", x => x.UserTypologyId);
                    table.ForeignKey(
                        name: "FK_UserTypology_Typology_TypologyId",
                        column: x => x.TypologyId,
                        principalTable: "Typology",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTypology_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ResidenceGeolocationId",
                table: "User",
                column: "ResidenceGeolocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesGeolocation_AttributesId",
                table: "AttributesGeolocation",
                column: "AttributesId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesGeolocation_GeolocationId",
                table: "AttributesGeolocation",
                column: "GeolocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesTypology_AttributesId",
                table: "AttributesTypology",
                column: "AttributesId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesTypology_TypologyId",
                table: "AttributesTypology",
                column: "TypologyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGeolocation_GeolocationId",
                table: "UserGeolocation",
                column: "GeolocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGeolocation_UserId",
                table: "UserGeolocation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypology_TypologyId",
                table: "UserTypology",
                column: "TypologyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTypology_UserId",
                table: "UserTypology",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Geolocation_ResidenceGeolocationId",
                table: "User",
                column: "ResidenceGeolocationId",
                principalTable: "Geolocation",
                principalColumn: "GeolocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Geolocation_ResidenceGeolocationId",
                table: "User");

            migrationBuilder.DropTable(
                name: "AttributesGeolocation");

            migrationBuilder.DropTable(
                name: "AttributesTypology");

            migrationBuilder.DropTable(
                name: "UserGeolocation");

            migrationBuilder.DropTable(
                name: "UserTypology");

            migrationBuilder.DropTable(
                name: "Geolocation");

            migrationBuilder.DropTable(
                name: "Typology");

            migrationBuilder.DropIndex(
                name: "IX_User_ResidenceGeolocationId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LatitudeUser",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LongitudeUser",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ResidenceGeolocationId",
                table: "User");
        }
    }
}
