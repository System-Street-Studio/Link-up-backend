using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrasturcture;
using Core.Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")] // This will map the URL to /api/users
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    // Dependency Injection: Injecting the AppDbContext to access the database
    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        // Simple endpoint to fetch all users from the Database
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound("User not found");

        return Ok(user);
    }

    // POST: api/users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User newUser)
    {
        // In a real app, you should hash the password before saving!
        newUser.CreatedAt = DateTime.UtcNow;

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        // Returns a 201 Created HTTP response
        return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
    }
}
