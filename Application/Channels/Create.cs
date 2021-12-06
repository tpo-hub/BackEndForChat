using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Channels
{
   public class Create
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class CommanValidator : AbstractValidator<Command>
        {
            public CommanValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var channel = new Channel()
                {
                    Description = request.Description,
                    Id = request.Id,
                    Name = request.Name,
                    ChannelType = ChannelType.Channel
                };
                context.Channels.Add(channel);
                var success = await context.SaveChangesAsync() > 0;

                if (success)
                {
                    return Unit.Value;
                }
                else 
                {
                    throw new Exception("Error al guardar los datos");
                }

            }
        }

    }
}
