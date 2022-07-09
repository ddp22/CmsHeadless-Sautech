using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class Add_ContentLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Content_Location_LocationId",
                table: "Content");

            migrationBuilder.DropIndex(
                name: "IX_Content_LocationId",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Content");

            migrationBuilder.CreateTable(
                name: "ContentLocation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentLocation_Content_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Content",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentLocation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentLocation_ContentId",
                table: "ContentLocation",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentLocation_LocationId",
                table: "ContentLocation",
                column: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentLocation");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Content",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Content_LocationId",
                table: "Content",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Location_LocationId",
                table: "Content",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId");
        }
    }
}
