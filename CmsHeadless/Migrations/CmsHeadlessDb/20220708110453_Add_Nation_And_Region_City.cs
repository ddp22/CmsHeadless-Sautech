using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class Add_Nation_And_Region_City : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Latitude",
            //    table: "Geolocation");

            //migrationBuilder.DropColumn(
            //    name: "Longitude",
            //    table: "Geolocation");

            //migrationBuilder.AddColumn<int>(
            //    name: "CityId1",
            //    table: "Geolocation",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "NationId1",
            //    table: "Geolocation",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "RegionId1",
            //    table: "Geolocation",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "GeolocationId",
            //    table: "Content",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "City",
            //    columns: table => new
            //    {
            //        CityId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_City", x => x.CityId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Nation",
            //    columns: table => new
            //    {
            //        NationId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        NationName = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Nation", x => x.NationId);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Region",
            //    columns: table => new
            //    {
            //        RegionId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        RegionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Region", x => x.RegionId);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Geolocation_CityId1",
            //    table: "Geolocation",
            //    column: "CityId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Geolocation_NationId1",
            //    table: "Geolocation",
            //    column: "NationId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Geolocation_RegionId1",
            //    table: "Geolocation",
            //    column: "RegionId1");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Content_GeolocationId",
            //    table: "Content",
            //    column: "GeolocationId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Content_Geolocation_GeolocationId",
            //    table: "Content",
            //    column: "GeolocationId",
            //    principalTable: "Geolocation",
            //    principalColumn: "GeolocationId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Geolocation_City_CityId1",
            //    table: "Geolocation",
            //    column: "CityId1",
            //    principalTable: "City",
            //    principalColumn: "CityId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Geolocation_Nation_NationId1",
            //    table: "Geolocation",
            //    column: "NationId1",
            //    principalTable: "Nation",
            //    principalColumn: "NationId",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Geolocation_Region_RegionId1",
            //    table: "Geolocation",
            //    column: "RegionId1",
            //    principalTable: "Region",
            //    principalColumn: "RegionId",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Content_Geolocation_GeolocationId",
            //    table: "Content");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Geolocation_City_CityId1",
            //    table: "Geolocation");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Geolocation_Nation_NationId1",
            //    table: "Geolocation");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Geolocation_Region_RegionId1",
            //    table: "Geolocation");

            //migrationBuilder.DropTable(
            //    name: "City");

            //migrationBuilder.DropTable(
            //    name: "Nation");

            //migrationBuilder.DropTable(
            //    name: "Region");

            //migrationBuilder.DropIndex(
            //    name: "IX_Geolocation_CityId1",
            //    table: "Geolocation");

            //migrationBuilder.DropIndex(
            //    name: "IX_Geolocation_NationId1",
            //    table: "Geolocation");

            //migrationBuilder.DropIndex(
            //    name: "IX_Geolocation_RegionId1",
            //    table: "Geolocation");

            //migrationBuilder.DropIndex(
            //    name: "IX_Content_GeolocationId",
            //    table: "Content");

            //migrationBuilder.DropColumn(
            //    name: "CityId1",
            //    table: "Geolocation");

            //migrationBuilder.DropColumn(
            //    name: "NationId1",
            //    table: "Geolocation");

            //migrationBuilder.DropColumn(
            //    name: "RegionId1",
            //    table: "Geolocation");

            //migrationBuilder.DropColumn(
            //    name: "GeolocationId",
            //    table: "Content");

            //migrationBuilder.AddColumn<double>(
            //    name: "Latitude",
            //    table: "Geolocation",
            //    type: "float",
            //    nullable: false,
            //    defaultValue: 0.0);

            //migrationBuilder.AddColumn<double>(
            //    name: "Longitude",
            //    table: "Geolocation",
            //    type: "float",
            //    nullable: false,
            //    defaultValue: 0.0);
        }
    }
}
