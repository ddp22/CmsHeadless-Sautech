using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class Add_Geocalocation_And_Typology_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserdId",
                table: "UserGeolocation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserdId",
                table: "UserGeolocation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
