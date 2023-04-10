using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Application.Dtos.V1.PlayerScores
{
    public class PlayerRankingListDto
    {
        public int Position { get; set; }
        public string Nick { get; set; }
        public int MatchesWon { get; set; }
        public int MatchesLost { get; set; }
        public int TotalGamesPlayed { get; set; }
        public double RankScore { get; set; }
    }
}
