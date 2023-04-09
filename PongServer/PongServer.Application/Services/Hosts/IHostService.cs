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
    public interface IHostService
    {
        Task<PagedResult<HostListDto>> GetAvailableHostsAsync(QueryFilters filters);
        Task<HostDetailsDto> GetHostByIdAsync(Guid hostId);
        Task<HostDetailsDto> CreateHostAsync(CreateHostDto createHostDto);
        Task<bool> JoinGameAsync(Guid hostId);
        Task<bool> DeleteHostAsync(Guid hostId);
    }
}
