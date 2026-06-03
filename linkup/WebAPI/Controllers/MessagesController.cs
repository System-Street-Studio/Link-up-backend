using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrasturcture;
using Core.Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly AppDbContext _context;

    public MessagesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/messages/room/{roomId}
    [HttpGet("room/{roomId}")]
    public async Task<IActionResult> GetRoomMessages(Guid roomId)
    {
        // Verify room exists
        var roomExists = await _context.ChatRooms.AnyAsync(r => r.Id == roomId);
        if (!roomExists)
            return NotFound("Chat room not found.");

        var messages = await _context.RoomMessages
            .Include(m => m.Sender)
            .Where(m => m.RoomId == roomId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        return Ok(messages);
    }

    // POST: api/messages
    [HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] RoomMessage newMessage)
    {
        // Verify room exists and is active
        var room = await _context.ChatRooms.FindAsync(newMessage.RoomId);
        if (room == null)
            return NotFound("Chat room not found.");

        if (!room.IsActive)
            return BadRequest("Cannot send message to an inactive chat room.");

        // Verify sender exists
        var senderExists = await _context.Users.AnyAsync(u => u.Id == newMessage.SenderId);
        if (!senderExists)
            return BadRequest("Sender does not exist.");

        // Additional validation: sender must be in the chat room
        if (room.UserOneId != newMessage.SenderId && room.UserTwoId != newMessage.SenderId)
            return BadRequest("Sender is not a member of this chat room.");

        newMessage.SentAt = DateTime.UtcNow;

        _context.RoomMessages.Add(newMessage);
        await _context.SaveChangesAsync();

        return Ok(newMessage);
    }
}
