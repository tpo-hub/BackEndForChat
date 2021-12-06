using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Channels
{
    public class List
    {
        public class Query : IRequest<List<Channel>>
        {
            public ChannelType ChannelType { get; set; } = ChannelType.Channel;
        }

        public class Handler : IRequestHandler<Query, List<Channel>>
        {
            private readonly DataContext context;

            public Handler(DataContext  context)
            {
                this.context = context;
            }
            public async Task<List<Channel>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.Channels
                   .Where(x=> x.ChannelType == ChannelType.Channel).ToListAsync();
            }
        }
    }
}
