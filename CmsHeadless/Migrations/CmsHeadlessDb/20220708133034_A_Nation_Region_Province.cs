using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class A_Nation_Region_Province : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Geolocation_GeolocationId",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Geolocation_ResidenceGeolocationId",
                table: "User");

            migrationBuilder.DropTable(
                name: "AttributesGeolocation");

            migrationBuilder.DropTable(
                name: "Geolocation");

            migrationBuilder.RenameColumn(
                name: "ResidenceGeolocationId",
                table: "User",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_User_ResidenceGeolocationId",
                table: "User",
                newName: "IX_User_LocationId");

            migrationBuilder.RenameColumn(
                name: "GeolocationId",
                table: "Attributes",
                newName: "LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Attributes_GeolocationId",
                table: "Attributes",
                newName: "IX_Attributes_LocationId");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Content",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Nation",
                columns: table => new
                {
                    NationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationIsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nation", x => x.NationId);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RegionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationId = table.Column<int>(type: "int", nullable: false),
                    RegionIsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionId);
                    table.ForeignKey(
                        name: "FK_Region_Nation_NationId",
                        column: x => x.NationId,
                        principalTable: "Nation",
                        principalColumn: "NationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Province",
                columns: table => new
                {
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvinceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    RegionIsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Province", x => x.ProvinceId);
                    table.ForeignKey(
                        name: "FK_Province_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NationId = table.Column<int>(type: "int", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    ProvinceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Location_Nation_NationId",
                        column: x => x.NationId,
                        principalTable: "Nation",
                        principalColumn: "NationId");
                    table.ForeignKey(
                        name: "FK_Location_Province_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Province",
                        principalColumn: "ProvinceId");
                    table.ForeignKey(
                        name: "FK_Location_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "RegionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Content_LocationId",
                table: "Content",
                column: "LocationId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Province_RegionId",
                table: "Province",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Region_NationId",
                table: "Region",
                column: "NationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Location_LocationId",
                table: "Attributes",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Content_Location_LocationId",
                table: "Content",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Location_LocationId",
                table: "User",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Location_LocationId",
                table: "Attributes");

            migrationBuilder.DropForeignKey(
                name: "FK_Content_Location_LocationId",
                table: "Content");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Location_LocationId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "Province");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Nation");

            migrationBuilder.DropIndex(
                name: "IX_Content_LocationId",
                table: "Content");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Content");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "User",
                newName: "ResidenceGeolocationId");

            migrationBuilder.RenameIndex(
                name: "IX_User_LocationId",
                table: "User",
                newName: "IX_User_ResidenceGeolocationId");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "Attributes",
                newName: "GeolocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Attributes_LocationId",
                table: "Attributes",
                newName: "IX_Attributes_GeolocationId");

            migrationBuilder.CreateTable(
                name: "Geolocation",
                columns: table => new
                {
                    GeolocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geolocation", x => x.GeolocationId);
                });

            migrationBuilder.CreateTable(
                name: "AttributesGeolocation",
                columns: table => new
                {
                    AttributesGeolocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributesId = table.Column<int>(type: "int", nullable: false),
                    GeolocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributesGeolocation", x => x.AttributesGeolocationId);
                    table.ForeignKey(
                        name: "FK_AttributesGeolocation_Attributes_AttributesId",
                        column: x => x.AttributesId,
                        principalTable: "Attributes",
                        principalColumn: "AttributesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttributesGeolocation_Geolocation_GeolocationId",
                        column: x => x.GeolocationId,
                        principalTable: "Geolocation",
                        principalColumn: "GeolocationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributesGeolocation_AttributesId",
                table: "AttributesGeolocation",
                column: "AttributesId");

            migrationBuilder.CreateIndex(
                name: "IX_AttributesGeolocation_GeolocationId",
                table: "AttributesGeolocation",
                column: "GeolocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Geolocation_GeolocationId",
                table: "Attributes",
                column: "GeolocationId",
                principalTable: "Geolocation",
                principalColumn: "GeolocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Geolocation_ResidenceGeolocationId",
                table: "User",
                column: "ResidenceGeolocationId",
                principalTable: "Geolocation",
                principalColumn: "GeolocationId");
        }
    }
}
