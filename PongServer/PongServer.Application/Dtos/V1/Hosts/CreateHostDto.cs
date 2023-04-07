using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PongServer.Application.Mappings;
using PongServer.Domain.Entities;

namespace PongServer.Application.Dtos.V1.Hosts
{
    public class CreateHostDto : IMap
    {
        public string Ip { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateHostDto, Host>();
        }
    }
}
