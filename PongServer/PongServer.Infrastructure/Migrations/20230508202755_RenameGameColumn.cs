using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PongServer.Infrastructure.Migrations
{
    public partial class RenameGameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_IdentityUser_GuestUserId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "GuestUserId",
                table: "Games",
                newName: "GuestPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_GuestUserId",
                table: "Games",
                newName: "IX_Games_GuestPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_IdentityUser_GuestPlayerId",
                table: "Games",
                column: "GuestPlayerId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_IdentityUser_GuestPlayerId",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "GuestPlayerId",
                table: "Games",
                newName: "GuestUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_GuestPlayerId",
                table: "Games",
                newName: "IX_Games_GuestUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_IdentityUser_GuestUserId",
                table: "Games",
                column: "GuestUserId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }
    }
}
