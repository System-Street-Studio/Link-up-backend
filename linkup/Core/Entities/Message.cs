using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class RoomMessage
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid RoomId { get; set; }
    
    [ForeignKey(nameof(RoomId))]
    public ChatRoom ChatRoom { get; set; } = null!;
    
    [Required]
    public Guid SenderId { get; set; }
    
    [ForeignKey(nameof(SenderId))]
    public User Sender { get; set; } = null!;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public string MessageType { get; set; } = "Text"; // Text
    
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}