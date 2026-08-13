using AuthLab.Api.Dtos;
using AuthLab.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthLab.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static readonly List<AppUser> _users = new()
        {
            new AppUser { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new AppUser { Id = 2, Name = "Bob", Email = "bob@example.com" },
            new AppUser { Id = 3, Name = "Charlie", Email = "charlie@example.com" }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create(UserDto userDto)
        {
            var newUser = new AppUser
            {
                Id = _users.Max(u => u.Id) + 1,
                Name = userDto.Name,
                Email = userDto.Email

            };

            _users.Add(newUser);

            //returns the resource url using the GetById method that is where to find the resource 
            return CreatedAtAction(nameof(GetById), new { id = newUser.Id }, newUser);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            _users.Remove(user);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, AppUser updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            return Ok(user);
        }



    }
}
