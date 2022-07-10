using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class Remove_Location_In_Attributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Location_LocationId",
                table: "Attributes");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_LocationId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Attributes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Attributes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_LocationId",
                table: "Attributes",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Location_LocationId",
                table: "Attributes",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId");
        }
    }
}
