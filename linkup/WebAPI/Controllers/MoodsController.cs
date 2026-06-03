using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrasturcture;
using Core.Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoodsController : ControllerBase
{
    private readonly AppDbContext _context;

    public MoodsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/moods
    [HttpGet]
    public async Task<IActionResult> GetMoods()
    {
        var moods = await _context.UserMoods.Include(m => m.User).ToListAsync();
        return Ok(moods);
    }

    // GET: api/moods/user/{userId}
    // Get moods by user ID
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetMoodsByUserId(Guid userId)
    {
        var userMoods = await _context.UserMoods
            .Include(m => m.User)
            .Where(m => m.UserId == userId)
            .ToListAsync();

        return Ok(userMoods);
    }

    // POST: api/moods
    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateMood([FromBody] UserMoode newMood)
    {
        // Check if the user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == newMood.UserId);
        if (!userExists)
            return BadRequest("User does not exist. Cannot update mood.");

        newMood.UpdatedAt = DateTime.UtcNow;

        _context.UserMoods.Add(newMood);
        await _context.SaveChangesAsync();

        return Ok(newMood);
    }
}
