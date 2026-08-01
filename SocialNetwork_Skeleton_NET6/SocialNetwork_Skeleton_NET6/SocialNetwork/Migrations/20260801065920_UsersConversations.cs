using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.Migrations
{
    public partial class UsersConversations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_Conversations_ConversationId",
                table: "UserConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserConversations_Users_UserId",
                table: "UserConversations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConversations",
                table: "UserConversations");

            migrationBuilder.RenameTable(
                name: "UserConversations",
                newName: "UsersConversations");

            migrationBuilder.RenameIndex(
                name: "IX_UserConversations_ConversationId",
                table: "UsersConversations",
                newName: "IX_UsersConversations_ConversationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersConversations",
                table: "UsersConversations",
                columns: new[] { "UserId", "ConversationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersConversations_Conversations_ConversationId",
                table: "UsersConversations",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersConversations_Users_UserId",
                table: "UsersConversations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersConversations_Conversations_ConversationId",
                table: "UsersConversations");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersConversations_Users_UserId",
                table: "UsersConversations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersConversations",
                table: "UsersConversations");

            migrationBuilder.RenameTable(
                name: "UsersConversations",
                newName: "UserConversations");

            migrationBuilder.RenameIndex(
                name: "IX_UsersConversations_ConversationId",
                table: "UserConversations",
                newName: "IX_UserConversations_ConversationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConversations",
                table: "UserConversations",
                columns: new[] { "UserId", "ConversationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_Conversations_ConversationId",
                table: "UserConversations",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserConversations_Users_UserId",
                table: "UserConversations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
