using AuthLab.Api.Data;
using AuthLab.Api.Dtos;
using AuthLab.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AuthLab.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<AppUser> _passwordHasher = new();


        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.Users
                .Select(u => new UserResponseDto { Id = u.Id, Email = u.Email, Name = u.Name })
                .ToListAsync();

            return Ok(result);
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync( u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(UserDto dto)
        {
            var newUser = new AppUser
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = "User"
            };
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, dto.Password);

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            //returns the resource url using the GetById method that is where to find the resource 
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user); //remove is not async we call SaveChangesAsync() to actully delete it 
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound();

            var callerId = User.FindFirst("sub")?.Value;
            var callerRole = User.FindFirst("role")?.Value;

            if (callerRole != "Admin" && callerId != id.ToString())
            {
                return Forbid(); // 403
            }

            user.Name = dto.Name;
            user.Email = dto.Email;
            await _context.SaveChangesAsync();

            return Ok(new UserResponseDto { Id = user.Id, Name = user.Name, Email = user.Email });
        }

    }
}
