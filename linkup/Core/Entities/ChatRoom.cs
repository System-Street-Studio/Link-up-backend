using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class ChatRoom
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid UserOneId { get; set; }
    public User UserOne { get; set; } = null!;
    
    [Required]
    public Guid UserTwoId { get; set; }
    public User UserTwo { get; set; } = null!;
    
    public bool MatchedByMood { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }
}