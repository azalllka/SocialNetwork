using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkNew.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Img",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Relationship",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorAuthentication",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Relationship = table.Column<int>(type: "integer", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Relationship",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TwoFactorAuthentication",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
