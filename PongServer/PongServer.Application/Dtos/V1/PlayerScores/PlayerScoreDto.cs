using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Enums;

namespace PongServer.Application.Dtos.V1.PlayerScores
{
    public class PlayerScoreDto
    {
        public Guid GameId { get; set; }
        public Guid OpponentId { get; set; }
        public GameResult MatchResult { get; set; }
    }
}
