using Microsoft.AspNetCore.Mvc;
using ServiceAbestraction;
using Shared.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController:ControllerBase
    {
        private readonly IManagerService manager;

        public AuthenticationController(IManagerService manager)
        {
            this.manager = manager;
        }
        [HttpPost("login")]
        public async Task<ActionResult<ReturnUserDto>> Login(LoginDto loginDto)
        {
            var result =await manager.authentication.Login(loginDto);
            return result;
        }
        [HttpPost("register")]
        public async Task<ActionResult<ReturnUserDto>> Register(RegisterDto userDto)
        {
            var result = await manager.authentication.Register(userDto);
            return result;
        }
        [HttpGet("emailexist")]
        public async Task<ActionResult<bool>> ChickEmailExist([FromQuery] string email)
        {
            var result = await manager.authentication.ChickEmailExist(email);
            return result;
        }

    }
}
