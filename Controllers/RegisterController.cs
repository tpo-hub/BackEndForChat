using Application.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class RegisterController : BaseController
    {
            [AllowAnonymous]
            [HttpPost]
            public async Task<ActionResult<UserDto>> Register(Register.Command command)
            {
                return await Mediator.Send(command);
            }
    }
}
