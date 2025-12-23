using Domain.Model;
using Domain.RepoInterface;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ServiceAbestraction;
using Shared.UserDto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> manager;
        private readonly IConfiguration configuration;

        public AuthenticationService(IUnitOfWork unitOfWork,UserManager<ApplicationUser> manager,
            IConfiguration configuration) 
        {
            this.unitOfWork = unitOfWork;
            this.manager = manager;
            this.configuration = configuration;
        }
        public async Task<bool> ChickEmailExist(string email)
        {
            var user =await manager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<ReturnUserDto> GetCurrentUser(string email)
        {
            var user=await manager.FindByEmailAsync(email);
            if (user == null) throw new Exception($"NotFound user With Email {email}");
            return new ReturnUserDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token=await GenerateToken(user)
            };
        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var claims= new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.DisplayName)
            };
            var rols= await manager.GetRolesAsync(user);
            foreach (var item in rols)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            var secretKey = configuration["JWT:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ReturnUserDto> Login(LoginDto loginDto)
        {
            var user =await manager.FindByEmailAsync(loginDto.Email);
            if (user == null) throw new Exception("Invalid Email or Password");
            var result = await manager.CheckPasswordAsync(user, loginDto.Password);
            if (!result) throw new Exception("Invalid Email or Password");
            return new ReturnUserDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token = await GenerateToken(user)
            };

        }

        public async Task<ReturnUserDto> Register(RegisterDto userDto)
        {
            var existingUser =await manager.FindByEmailAsync(userDto.Email);
            if (existingUser != null) throw new Exception("Email is already taken");
            var user = new ApplicationUser
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                DisplayName = userDto.Username,
                PhoneNumber = userDto.phoneNumber
            };
            var result = await manager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User registration failed: {errors}");
            }
            return new ReturnUserDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token = await GenerateToken(user)
            };
        }
    }
}
