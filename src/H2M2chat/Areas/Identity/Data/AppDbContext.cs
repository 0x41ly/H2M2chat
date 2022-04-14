using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using H2M2chat.Models;

namespace H2M2chat.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<H2M2chat.Models.Topic> Topic { get; set; }

    public DbSet<H2M2chat.Models.Comment> Comment { get; set; }

    public DbSet<H2M2chat.Models.Room> Room { get; set; }
    public DbSet<H2M2chat.Models.Message> Message { get; set; }
}
