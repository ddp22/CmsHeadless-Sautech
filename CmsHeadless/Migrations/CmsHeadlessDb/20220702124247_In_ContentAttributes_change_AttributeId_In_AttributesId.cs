using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class In_ContentAttributes_change_AttributeId_In_AttributesId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeId",
                table: "ContentAttributes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttributeId",
                table: "ContentAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
