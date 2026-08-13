using AuthLab.Api.Dtos;
using AuthLab.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthLab.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly List<AppUser> _users = new();
        private readonly PasswordHasher<AppUser> _passwordHasher = new();

        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            var newUser = new AppUser
            {
                Id = _users.Count + 1,
                Name = dto.Name,
                Email = dto.email,
                Role = "User"
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.password);
            _users.Add(newUser);

            // use response dto to only expose the fields that you want to be exposed to the user
            var response = new UserResponseDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email
            };

            return Ok(response);


        }
    }
}
