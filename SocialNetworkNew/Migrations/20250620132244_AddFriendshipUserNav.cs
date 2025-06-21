using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkNew.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendshipUserNav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_UserId",
                table: "Friendships",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_UserId",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_UserId",
                table: "Friendships");
        }
    }
}
