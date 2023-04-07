using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Entities;
using PongServer.Domain.Interfaces;

namespace PongServer.Infrastructure.Repositories
{
    public class HostRepository : IHostRepository
    {
        public async Task<IEnumerable<Host>> GetAvailableHostsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Host> GetHostByIdAsync(Guid hostId)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateHostAsync(Host host)
        {
            throw new NotImplementedException();
        }

        public async Task HideHostAsync(Host host)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteHostAsync(Host host)
        {
            throw new NotImplementedException();
        }
    }
}
