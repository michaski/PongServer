using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PongServer.Infrastructure.Migrations
{
    public partial class AddGameEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HostId = table.Column<Guid>(type: "uuid", nullable: false),
                    HostPlayerId = table.Column<string>(type: "text", nullable: true),
                    GuestUserId = table.Column<string>(type: "text", nullable: true),
                    HostPlayerScore = table.Column<int>(type: "integer", nullable: false),
                    GuestPlayerScore = table.Column<int>(type: "integer", nullable: false),
                    GameStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_IdentityUser_GuestUserId",
                        column: x => x.GuestUserId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_IdentityUser_HostPlayerId",
                        column: x => x.HostPlayerId,
                        principalTable: "IdentityUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_GuestUserId",
                table: "Games",
                column: "GuestUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_HostId",
                table: "Games",
                column: "HostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_HostPlayerId",
                table: "Games",
                column: "HostPlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
