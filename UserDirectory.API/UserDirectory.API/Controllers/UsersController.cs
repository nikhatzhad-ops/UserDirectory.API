using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDirectory.Application.DTOs;
using UserDirectory.Domain.Models;
using UserDirectory.Infrastructure.Data;

namespace UserDirectory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) => _db = db;

        // GET /api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _db.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }

        // GET /api/users/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST /api/users
        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // PUT /api/users/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var existing = await _db.Users.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.Phone = dto.Phone;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /api/users/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Users.FindAsync(id);
            if (item == null) return NotFound();

            _db.Users.Remove(item);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
