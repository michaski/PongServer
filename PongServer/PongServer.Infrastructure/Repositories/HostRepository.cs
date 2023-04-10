using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PongServer.Domain.Entities;
using PongServer.Domain.Interfaces;
using PongServer.Domain.Utils;
using PongServer.Infrastructure.Data;
using PongServer.Infrastructure.Extensions;

namespace PongServer.Infrastructure.Repositories
{
    public class HostRepository : IHostRepository
    {
        private readonly PongDataContext _context;

        public HostRepository(PongDataContext context)
        {
            _context = context;
        }

        public async Task<ResultPage<Host>> GetAvailableHostsAsync(QueryFilters filters)
        {
            var results = await _context.Hosts
                .Where(host => host.IsAvailable == true)
                .SearchInField(host => host.Name, filters.HostName)
                .OrderBy(host => host.Name, filters.Ordering)
                .ToResultPageAsync(filters);

            return results;
        }

        public async Task<Host> GetHostByIdAsync(Guid hostId)
        {
            return await _context.Hosts
                .FirstOrDefaultAsync(host => host.Id == hostId);
        }

        public async Task<Host> GetHostByNameAsync(string hostName)
        {
            return await _context.Hosts
                .FirstOrDefaultAsync(host => host.Name.ToLower() == hostName.ToLower());
        }

        public async Task<Host> CreateHostAsync(Host host)
        {
            await _context.Hosts.AddAsync(host);
            await _context.SaveChangesAsync();
            return host;
        }

        public async Task HideHostAsync(Host host)
        {
            host.IsAvailable = false;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHostAsync(Host host)
        {
            _context.Hosts.Remove(host);
            await _context.SaveChangesAsync();
        }
    }
}
