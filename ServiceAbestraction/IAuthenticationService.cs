using Shared.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbestraction
{
    public interface IAuthenticationService
    {
        Task<ReturnUserDto> Register(RegisterDto userDto);
        Task<ReturnUserDto> Login(LoginDto loginDto);
        Task<bool> ChickEmailExist(string email);
        Task<ReturnUserDto> GetCurrentUser(string email);
    }
}
