using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PongServer.Domain.Entities
{
    public class PlayerScore
    {
        public Guid Id { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int RankingScore { get; set; }
        public IdentityUser Player { get; set; }
    }
}
