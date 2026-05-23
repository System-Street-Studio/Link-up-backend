using Microsoft.EntityFrameworkCore;
using Core.Entities;  

namespace Infrasturcture;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<UserMoode> UserMoods => Set<UserMoode>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
    public DbSet<RoomMessage> RoomMessages => Set<RoomMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ChatRoom -> UserOne (Foreign Key Relationship Config)
        modelBuilder.Entity<ChatRoom>()
            .HasOne(r => r.UserOne)
            .WithMany()
            .HasForeignKey(r => r.UserOneId)
            .OnDelete(DeleteBehavior.Restrict);

        // ChatRoom -> UserTwo (Foreign Key Relationship Config)
        modelBuilder.Entity<ChatRoom>()
            .HasOne(r => r.UserTwo)
            .WithMany()
            .HasForeignKey(r => r.UserTwoId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}