using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkNew.Migrations
{
    /// <inheritdoc />
    public partial class FinalSyncMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialLink",
                table: "SocialLink");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notification",
                table: "Notification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Event",
                table: "Event");

            migrationBuilder.RenameTable(
                name: "SocialLink",
                newName: "SocialLinks");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "Notification",
                newName: "Notifications");

            migrationBuilder.RenameTable(
                name: "Event",
                newName: "Events");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialLinks",
                table: "SocialLinks",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SocialLinks",
                table: "SocialLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "SocialLinks",
                newName: "SocialLink");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notification");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "Event");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SocialLink",
                table: "SocialLink",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notification",
                table: "Notification",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Event",
                table: "Event",
                column: "EventId");
        }
    }
}
