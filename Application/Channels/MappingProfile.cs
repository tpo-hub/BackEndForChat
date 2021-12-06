using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Channels
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Channel, ChannelDto>();
        }
    }
}
