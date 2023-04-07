using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PongServer.Domain.Entities
{
    public class Host
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool IsAvailable { get; set; }
        public IdentityUser Owner { get; set; }
    }
}
