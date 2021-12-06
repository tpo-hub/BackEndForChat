using Application.Errors;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Messages
{
    public class Create
    {
        public class Command : IRequest<MessageDto>
        {
            public string Content { get; set; }
            public Guid ChannelId { get; set; }
            public MessageType MessageType { get; set; } = MessageType.Text;
            public IFormFile formFile { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.ChannelId).NotEmpty();
            }
        }
        public class Handler : IRequestHandler<Command, MessageDto>
        {
            private readonly DataContext context;
            private readonly IUserAccessor userAccessor;
            private readonly IMapper mapper;
            private readonly IMediaUpload mediaUpload;

            public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper,
                IMediaUpload mediaUpload )
            {
                this.context = context;
                this.userAccessor = userAccessor;
                this.mapper = mapper;
                this.mediaUpload = mediaUpload;
            }
            public async Task<MessageDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await context.Users
                    .SingleOrDefaultAsync(x => x.UserName == userAccessor.GetCurrentUserName());
                var channel = await context.Channels.SingleOrDefaultAsync(x => x.Id == request.ChannelId);

                if(channel == null)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { channel = "the channel not found" });
                }

                var message = new Message
                {
                    Content = request.MessageType == MessageType.Text ? request.Content : 
                    mediaUpload.UploadMedia(request.formFile).Url,
                    Channel = channel,
                    Sender = user,
                    CreartedAt = DateTime.Now,
                    MessageType = request.MessageType
                };

                context.Messages.Add(message);
                try
                {
                    var save = await context.SaveChangesAsync();
                    return mapper.Map<MessageDto>(message);
                    
                }
                catch 
                {
                    throw new RestException(HttpStatusCode.BadRequest,
                        new { Message = "create message error" });
                }

            }
        }
    }
}
