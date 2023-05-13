using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PongServer.Domain.Entities;
using PongServer.Domain.Utils;

namespace PongServer.Domain.Interfaces
{
    public interface IHostRepository
    {
        Task<ResultPage<Host>> GetAvailableHostsAsync(QueryFilters filters);
        Task<Host> GetHostByIdAsync(Guid hostId);
        Task<Host> GetHostByNameAsync(string hostName);
        Task<bool> UserIsHostingGameAsync(string userId);
        Task<Host> CreateHostAsync(Host host);
        Task HideHostAsync(Host host);
        Task DeleteHostAsync(Host host);
    }
}
