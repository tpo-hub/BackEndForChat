using Application.Errors;
using Application.Interfaces;
using AutoMapper;
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
   public class PrivateChannelDetails
    {
        public class Query : IRequest<ChannelDto>
        {
            public string UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, ChannelDto>
        {
            private readonly DataContext context;
            private readonly IMapper mapper;
            private readonly IUserAccessor userAccessor;

            public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor )
            {
                this.context = context;
                this.mapper = mapper;
                this.userAccessor = userAccessor;
            }
            public async Task<ChannelDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUser = await context.Users.SingleOrDefaultAsync(x =>
                x.UserName == userAccessor.GetCurrentUserName());

                var user = await context.Users.FindAsync(request.UserId);

                if(user == null)
                {
                    throw new RestException(System.Net.HttpStatusCode.BadRequest, new
                    { Error = " the user to send doesnt exist  " });
                   
                }    

                var privateChannelIdXCurrentUser = GetPrivateChannelId(currentUser.Id.ToString(),
                    request.UserId);

                var privateChannelIdXRecipientUser = GetPrivateChannelId(request.UserId, 
                    currentUser.Id.ToString());

                var channel = await context.Channels.Include(x => x.Messages).ThenInclude(x => x.Sender)
                    .SingleOrDefaultAsync(x => x.PrivateChannelId == privateChannelIdXCurrentUser  
                    || x.PrivateChannelId ==  privateChannelIdXRecipientUser);
               
                if(channel == null)
                {
                    var newChannel = new Channel
                    {
                        Id = Guid.NewGuid(),
                        Name = currentUser.UserName,
                        Description = user.UserName,
                        ChannelType = ChannelType.Room,
                        PrivateChannelId = privateChannelIdXCurrentUser
                    };

                    context.Channels.Add(newChannel);
                    var success = await context.SaveChangesAsync() > 0;

                    if(success)
                    {
                        return mapper.Map<Channel, ChannelDto>(newChannel);
                    }
                }

                return mapper.Map<Channel, ChannelDto>(channel);

            }

            private string GetPrivateChannelId(string currentUserId, string userId)
            {
                return $"{currentUserId}/{userId}";
            }
        }

    }
}
