using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class Delete_virtual_In_Location : Migration
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
