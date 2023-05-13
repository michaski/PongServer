using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PongServer.Domain.Entities;

namespace PongServer.Infrastructure.Data
{
    public class PongDataContext : DbContext
    {
        public DbSet<Host> Hosts { get; set; }
        public DbSet<PlayerScore> Scores { get; set; }
        public DbSet<Game> Games { get; set; }

        public PongDataContext(DbContextOptions<PongDataContext> options)
            : base(options) { }
    }
}
