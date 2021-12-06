using Application.Errors;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Channels
{
    public class Details 
    {
        public class Query : IRequest<ChannelDto>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, ChannelDto>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }
   
            public async Task<ChannelDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var channelDB = await
                    context.Channels.Include(x=> x.Messages)
                        .ThenInclude(x=> x.Sender)
                      .FirstOrDefaultAsync(x => x.Id == request.Id);

                var channel = mapper.Map<ChannelDto>(channelDB) ;
                if(channel == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new
                    {
                        channel = "the chanel does not exist"
                    } );
                }
            
                return channel;
            }
        }
    }

}

