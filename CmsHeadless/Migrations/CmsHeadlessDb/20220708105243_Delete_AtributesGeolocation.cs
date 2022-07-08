using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class Delete_AtributesGeolocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GeolocationId",
                table: "Attributes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_GeolocationId",
                table: "Attributes",
                column: "GeolocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Geolocation_GeolocationId",
                table: "Attributes",
                column: "GeolocationId",
                principalTable: "Geolocation",
                principalColumn: "GeolocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Geolocation_GeolocationId",
                table: "Attributes");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_GeolocationId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "GeolocationId",
                table: "Attributes");
        }
    }
}
