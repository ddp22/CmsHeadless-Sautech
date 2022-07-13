using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class _20220710145726_Remove_IsNone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNone",
                table: "Region");

            migrationBuilder.DropColumn(
                name: "IsNone",
                table: "Province");

            migrationBuilder.DropColumn(
                name: "IsNone",
                table: "Nation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNone",
                table: "Region",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNone",
                table: "Province",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNone",
                table: "Nation",
                type: "bit",
                nullable: true);
        }
    }
}
