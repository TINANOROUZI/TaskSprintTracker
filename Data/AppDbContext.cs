using Microsoft.EntityFrameworkCore;
using TaskSprintTracker.Models;

namespace TaskSprintTracker.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Project>().HasMany(p => p.Tasks).WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);

        b.Entity<TaskItem>().Property(t => t.Status).HasConversion<string>();
        b.Entity<TaskItem>().Property(t => t.Priority).HasConversion<string>();
    }
}
