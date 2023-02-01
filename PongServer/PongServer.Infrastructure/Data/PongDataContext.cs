using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PongServer.Infrastructure.Data
{
    public class PongDataContext : DbContext
    {
        public PongDataContext(DbContextOptions options)
            : base(options) { }
    }
}
