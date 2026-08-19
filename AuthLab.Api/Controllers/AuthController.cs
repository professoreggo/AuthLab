using AuthLab.Api.Dtos;
using AuthLab.Api.Models;
using AuthLab.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthLab.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        // readonly means that once initiazlied you can not change; used with injected services too;
        // it applies to the reference not the object reference
        private static readonly List<AppUser> _users = new();
        private readonly PasswordHasher<AppUser> _passwordHasher = new();
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;  // DI for token service
        }


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

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = _users.FirstOrDefault(u => u.Email == dto.Email);

            // user not exist
            if (user == null)
            {
                return Unauthorized(); // 401
            }

            // password incorrect
            var passResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized(); // 401 too this is called user enumeration
            }

            var token = _tokenService.CreateToken(user);

            return Ok(new { accessToken = token });

            //return Ok(new UserResponseDto
            //{
            //    Id = user.Id,
            //    Name = user.Name,
            //    Email = user.Email,
            //});





        }
    }
}
