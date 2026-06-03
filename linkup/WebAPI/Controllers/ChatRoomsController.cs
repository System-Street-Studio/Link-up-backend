using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrasturcture;
using Core.Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatRoomsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChatRoomsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/chatrooms
    [HttpGet]
    public async Task<IActionResult> GetChatRooms()
    {
        var chatRooms = await _context.ChatRooms
            .Include(c => c.UserOne)
            .Include(c => c.UserTwo)
            .ToListAsync();

        return Ok(chatRooms);
    }

    // GET: api/chatrooms/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserChatRooms(Guid userId)
    {
        var userRooms = await _context.ChatRooms
            .Include(c => c.UserOne)
            .Include(c => c.UserTwo)
            .Where(c => c.UserOneId == userId || c.UserTwoId == userId)
            .ToListAsync();

        return Ok(userRooms);
    }

    // POST: api/chatrooms
    [HttpPost]
    public async Task<IActionResult> CreateChatRoom([FromBody] ChatRoom newRoom)
    {
        // Ensure both users exist
        var userOneExists = await _context.Users.AnyAsync(u => u.Id == newRoom.UserOneId);
        var userTwoExists = await _context.Users.AnyAsync(u => u.Id == newRoom.UserTwoId);

        if (!userOneExists || !userTwoExists)
            return BadRequest("One or both users do not exist.");

        newRoom.CreatedAt = DateTime.UtcNow;
        newRoom.IsActive = true;

        _context.ChatRooms.Add(newRoom);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserChatRooms), new { userId = newRoom.UserOneId }, newRoom);
    }

    // PUT: api/chatrooms/{id}/end
    [HttpPut("{id}/end")]
    public async Task<IActionResult> EndChatRoom(Guid id)
    {
        var chatRoom = await _context.ChatRooms.FindAsync(id);
        if (chatRoom == null)
            return NotFound("Chat room not found.");

        chatRoom.IsActive = false;
        chatRoom.EndedAt = DateTime.UtcNow;

        _context.ChatRooms.Update(chatRoom);
        await _context.SaveChangesAsync();

        return Ok(chatRoom);
    }
}
