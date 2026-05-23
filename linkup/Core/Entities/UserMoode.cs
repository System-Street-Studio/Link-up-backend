using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class UserMoode
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    [Required]
    public string MoodType { get; set; } = string.Empty;
    
    public string? FleetingThought { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}