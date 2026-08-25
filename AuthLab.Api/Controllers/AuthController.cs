using AuthLab.Api.Data;
using AuthLab.Api.Dtos;
using AuthLab.Api.Models;
using AuthLab.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuthLab.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        // readonly means that once initiazlied you can not change; used with injected services too;
        // it applies to the reference not the object reference
        private readonly AppDbContext _context;
        private readonly PasswordHasher<AppUser> _passwordHasher = new();
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService, AppDbContext context)
        {
            _context = context;
            _tokenService = tokenService;  // DI for token service
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.email);
            if (emailExists)
            {
                return Conflict();
            }
            var newUser = new AppUser
            {
                Name = dto.Name,
                Email = dto.email,
                Role = "User"
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.password);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();


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
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var checkPass = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (checkPass == PasswordVerificationResult.Failed)
            {
                return Unauthorized();
            }

            var token = _tokenService.CreateToken(user);
            return Ok(new { accessToken = token });


        }

        //[HttpPost("make-admin/{id}")]
        //public IActionResult MakeAdmin(int id)
        //{
        //    var user = _users.FirstOrDefault(u => u.Id == id);
        //    if (user == null) return NotFound();

        //    user.Role = "Admin";
        //    return Ok(new { user.Id, user.Name, user.Role });
        //}
    }
}
