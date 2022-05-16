using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain;

namespace Persistence;

public class DataContext : IdentityDbContext<ApplicationUser>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoItem>()
                    .HasOne(todoItem => todoItem.CreatedBy)
                    .WithMany()
                    .HasForeignKey(todoItem => todoItem.CreatedById)
                    .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Comment>()
                    .HasOne(comment => comment.TodoItem)
                    .WithMany(todoItem => todoItem.Comments)
                    .HasForeignKey(comment => comment.TodoItemId)
                    .OnDelete(DeleteBehavior.Cascade);
    }
}
