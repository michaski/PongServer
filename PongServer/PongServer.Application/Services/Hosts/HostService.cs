using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Application.Dtos.V1.Hosts;
using PongServer.Application.Dtos.V1.Pagination;
using PongServer.Domain.Utils;

namespace PongServer.Application.Services.Hosts
{
    public class HostService : IHostService
    {
        public async Task<PagedResult<HostListDto>> GetAvailableHostsAsync(QueryFilters filters)
        {
            throw new NotImplementedException();
        }

        public async Task<HostDetailsDto> GetHostByIdAsync(Guid hostId)
        {
            throw new NotImplementedException();
        }

        public async Task<HostDetailsDto> CreateHostAsync(CreateHostDto createHostDto)
        {
            throw new NotImplementedException();
        }

        public async Task JoinGameAsync(Guid hostId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteHostAsync(Guid hostId)
        {
            throw new NotImplementedException();
        }
    }
}
