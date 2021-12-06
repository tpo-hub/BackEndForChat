using Application.User;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> Current()
        {
            return await Mediator.Send(new CurrenUser.Query());
        }   
        
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<UserDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }  
        
        [HttpGet("logout/{id}")]
        public async Task<Unit> logout(string id)
        {
            return await Mediator.Send(new Logout.Query { UserId = id});
        }


    }
}
