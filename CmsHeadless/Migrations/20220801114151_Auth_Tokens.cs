using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsHeadless.Migrations
{
    public partial class Auth_Tokens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthTokensUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTokens", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AuthTokensUserId",
                table: "AspNetUsers",
                column: "AuthTokensUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AuthTokens_AuthTokensUserId",
                table: "AspNetUsers",
                column: "AuthTokensUserId",
                principalTable: "AuthTokens",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AuthTokens_AuthTokensUserId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AuthTokens");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AuthTokensUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AuthTokensUserId",
                table: "AspNetUsers");
        }
    }
}
