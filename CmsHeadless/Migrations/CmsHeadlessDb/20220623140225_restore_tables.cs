using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations.CmsHeadlessDb
{
    public partial class restore_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ContentId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "key",
                table: "Attributes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryParentId",
                table: "Category",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AttributeName",
                table: "Attributes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AttributeValue",
                table: "Attributes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ContentAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    AttributeId = table.Column<int>(type: "int", nullable: false),
                    AttributesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentAttributes_Attributes_AttributesId",
                        column: x => x.AttributesId,
                        principalTable: "Attributes",
                        principalColumn: "AttributesId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentAttributes_Content_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Content",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentCategory_Content_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Content",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentTag_Content_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Content",
                        principalColumn: "ContentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentAttributes_AttributesId",
                table: "ContentAttributes",
                column: "AttributesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentAttributes_ContentId",
                table: "ContentAttributes",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentCategory_CategoryId",
                table: "ContentCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentCategory_ContentId",
                table: "ContentCategory",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTag_ContentId",
                table: "ContentTag",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentTag_TagId",
                table: "ContentTag",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentAttributes");

            migrationBuilder.DropTable(
                name: "ContentCategory");

            migrationBuilder.DropTable(
                name: "ContentTag");

            migrationBuilder.DropColumn(
                name: "AttributeName",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "AttributeValue",
                table: "Attributes");

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "Tag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryParentId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ContentId",
                table: "Attributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Attributes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "key",
                table: "Attributes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
