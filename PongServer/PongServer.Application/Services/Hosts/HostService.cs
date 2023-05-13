using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PongServer.Application.Dtos.LayerResult;
using PongServer.Application.Dtos.V1.Hosts;
using PongServer.Application.Dtos.V1.Pagination;
using PongServer.Application.Services.UserContext;
using PongServer.Domain.Entities;
using PongServer.Domain.Interfaces;
using PongServer.Domain.Utils;

namespace PongServer.Application.Services.Hosts
{
    public class HostService : IHostService
    {
        private readonly IHostRepository _hostRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserContextService _userContextService;

        public HostService(
            IHostRepository hostRepository, 
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IUserContextService userContextService)
        {
            _hostRepository = hostRepository;
            _mapper = mapper;
            _userManager = userManager;
            _userContextService = userContextService;
        }

        public async Task<PagedResult<HostListDto>> GetAvailableHostsAsync(QueryFilters filters)
        {
            var queryResult = await _hostRepository.GetAvailableHostsAsync(filters);
            var mappedHosts = _mapper.Map<IEnumerable<Host>, IEnumerable<HostListDto>>(queryResult.Items);
            var resultPage = new ResultPage<HostListDto>(mappedHosts, queryResult.TotalItemsCount);
            return new PagedResult<HostListDto>(resultPage, filters);
        }

        public async Task<HostDetailsDto> GetHostByIdAsync(Guid hostId)
        {
            var foundHost = await _hostRepository.GetHostByIdAsync(hostId);
            return _mapper.Map<Host, HostDetailsDto>(foundHost);
        }

        public async Task<ApplicationResult<HostDetailsDto>> CreateHostAsync(CreateHostDto createHostDto)
        {
            var userIsAlreadyHosting = await _hostRepository.UserIsHostingGameAsync(_userContextService.UserId);
            if (userIsAlreadyHosting)
            {
                return new ApplicationResult<HostDetailsDto>
                {
                    Status = 400,
                    Message = "You are already hosting a game."
                };
            }

            var nameCheckHost = await _hostRepository.GetHostByNameAsync(createHostDto.Name);
            if (nameCheckHost != null)
            {
                return new ApplicationResult<HostDetailsDto>
                {
                    Status = 400,
                    Message = "This name is already taken."
                };
            }

            var hostInfo = _mapper.Map<CreateHostDto, Host>(createHostDto);
            var owner = await _userManager.FindByIdAsync(_userContextService.UserId);
            hostInfo.Owner = owner;
            hostInfo.IsAvailable = true;
            var createdHost = await _hostRepository.CreateHostAsync(hostInfo);
            var mappedHost = _mapper.Map<Host, HostDetailsDto>(createdHost);
            return new ApplicationResult<HostDetailsDto>()
            {
                Status = 203,
                Result = mappedHost
            };
        }

        public async Task<ApplicationResult<bool>> JoinGameAsync(Guid hostId)
        {
            var hostToHide = await _hostRepository.GetHostByIdAsync(hostId);
            if (hostToHide is null)
            {
                return new ApplicationResult<bool>
                {
                    Status = 400,
                    Message = "Host was not found"
                };
            }
            if (hostToHide.Owner.Id == _userContextService.UserId)
            {
                return new ApplicationResult<bool>
                {
                    Status = 400,
                    Message = "User can't join his own host"
                };
            }
            await _hostRepository.HideHostAsync(hostToHide);
            return new ApplicationResult<bool>
            {
                Status = 200
            };
        }

        public async Task<bool> DeleteHostAsync(Guid hostId)
        {
            var hostToDelete = await _hostRepository.GetHostByIdAsync(hostId);
            if (hostToDelete is null)
            {
                return false;
            }
            await _hostRepository.DeleteHostAsync(hostToDelete);
            return true;
        }
    }
}
