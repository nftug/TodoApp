using Microsoft.EntityFrameworkCore;
using Domain;

namespace Persistence
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                        .HasOne(comment => comment.TodoItem)
                        .WithMany(todoItem => todoItem.Comments)
                        .HasForeignKey(comment => comment.TodoItemId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}