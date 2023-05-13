using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServer.Application.Dtos.V1.Games
{
    public class UpdateScoreDto
    {
        public Guid GameId { get; set; }
        public bool HostWon { get; set; }
    }
}
