using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Channels;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace API.Controllers
{
   
    public class ChannelsController : BaseController
    {
        public ChannelsController() : base()
        {
        }

        [HttpGet]
        public async Task<ActionResult<List<Channel>>> Channels()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChannelDto>> Details(Guid id)
        {
            return await Mediator.Send(new Details.Query() { 
             Id = id
            });
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create([FromBody] Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("private/{id}")]
        public async Task<ActionResult<ChannelDto>> PrivateDetails(string id)
        {
            return await Mediator.Send(new PrivateChannelDetails.Query { 
                 UserId = id
            });
        }

    }
}