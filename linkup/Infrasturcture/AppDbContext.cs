using Microsoft.EntityFrameworkCore;
//using Core.Entities;  💡 පසුවට Core එකේ සාදන Entities (User, Post වැනි) හඳුනා ගැනීමට

namespace Infrasturcture;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // 💡 ඔයාගේ ER Diagram එකේ තියෙන Tables ටික මෙතන register කරනවා
    // (දැනට Entities classes හදලා නැති නිසා මේවා comment කරලා තියන්න, පස්සේ comment අයින් කරමු)
    // public DbSet<User> Users => Set<User>();
    // public DbSet<UserMood> UserMoods => Set<UserMood>();
    // public DbSet<Post> Posts => Set<Post>();
    // public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
    // public DbSet<RoomMessage> RoomMessages => Set<RoomMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // පසුවට ER Diagram එකේ තියෙන සංකීර්ණ සබඳතා (Fluent API Configurations)
        // ලියන්නේ මේ ඇතුලෙයි.
    }
}