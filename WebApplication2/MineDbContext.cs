using Microsoft.EntityFrameworkCore;
using WebApplication2.RequestModels;

namespace WebApplication2;

public class MineDbContext : DbContext
{
    public MineDbContext(DbContextOptions<MineDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<VibrationData> VibrationDatas { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Device> Devices { get; set; }
}   
