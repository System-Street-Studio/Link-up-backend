using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrasturcture;
using Core.Entities;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PostsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/posts
    [HttpGet]
    public async Task<IActionResult> GetPosts()
    {
        // Include the User info when fetching posts
        var posts = await _context.Posts.Include(p => p.User).ToListAsync();
        return Ok(posts);
    }

    // GET: api/posts/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var post = await _context.Posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);

        if (post == null)
            return NotFound("Post not found");

        return Ok(post);
    }

    // POST: api/posts
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] Post newPost)
    {
        // Check if the user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == newPost.UserId);
        if (!userExists)
            return BadRequest("User does not exist. Cannot create post.");

        newPost.CreatedAt = DateTime.UtcNow;
        newPost.UpdatedAt = DateTime.UtcNow;

        _context.Posts.Add(newPost);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPostById), new { id = newPost.Id }, newPost);
    }

    // DELETE: api/posts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(Guid id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
            return NotFound("Post not found");

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
