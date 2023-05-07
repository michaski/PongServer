using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PongServer.Infrastructure.Migrations
{
    public partial class AddRankingPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RankingPosition",
                table: "Scores",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RankingPosition",
                table: "Scores");
        }
    }
}
