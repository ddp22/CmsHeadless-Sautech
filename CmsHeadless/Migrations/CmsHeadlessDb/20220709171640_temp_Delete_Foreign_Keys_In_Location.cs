using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class temp_Delete_Foreign_Keys_In_Location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Nation_NationId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Province_ProvinceId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Region_RegionId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_NationId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_ProvinceId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_RegionId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "NationId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NationId",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_NationId",
                table: "Location",
                column: "NationId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_ProvinceId",
                table: "Location",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_RegionId",
                table: "Location",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Nation_NationId",
                table: "Location",
                column: "NationId",
                principalTable: "Nation",
                principalColumn: "NationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Province_ProvinceId",
                table: "Location",
                column: "ProvinceId",
                principalTable: "Province",
                principalColumn: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Region_RegionId",
                table: "Location",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "RegionId");
        }
    }
}
