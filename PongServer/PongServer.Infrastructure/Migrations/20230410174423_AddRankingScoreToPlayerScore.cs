using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PongServer.Infrastructure.Migrations
{
    public partial class AddRankingScoreToPlayerScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Scores",
                newName: "GamesWon");

            migrationBuilder.AddColumn<int>(
                name: "RankingScore",
                table: "Scores",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RankingScore",
                table: "Scores");

            migrationBuilder.RenameColumn(
                name: "GamesWon",
                table: "Scores",
                newName: "Score");
        }
    }
}
