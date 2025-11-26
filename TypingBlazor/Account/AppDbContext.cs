namespace TypingBlazor;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<TypingUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
       // Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<TypingUser>().OwnsOne(u => u.StatisticsOfLastTraining);
    }
}