using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Entities;

namespace PongServer.Domain.Interfaces
{
    public interface IHostRepository
    {
        Task<IEnumerable<Host>> GetAvailableHostsAsync();
        Task<Host> GetHostByIdAsync(Guid hostId);
        Task<Guid> CreateHostAsync(Host host);
        Task HideHostAsync(Host host);
        Task DeleteHostAsync(Host host);
    }
}
