using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PongServer.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public Guid HostId { get; set; }
        public Host Host { get; set; }
        public IdentityUser HostPlayer { get; set; }
        public IdentityUser GuestPlayer { get; set; }
        public int HostPlayerScore { get; set; }
        public int GuestPlayerScore { get; set; }
        public DateTime GameStartTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
