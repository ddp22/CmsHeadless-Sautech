using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class changeNameCategory_to_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_LicensePlate",
                table: "Category",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_LicensePlate",
                table: "Category");
        }
    }
}
