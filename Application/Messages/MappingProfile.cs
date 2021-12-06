using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messages
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageDto >();
        }
    }
}
