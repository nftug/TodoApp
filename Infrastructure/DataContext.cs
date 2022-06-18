using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.DataModels;

namespace Infrastructure;

public class DataContext : IdentityDbContext<UserDataModel>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<TodoDataModel> Todos { get; set; } = null!;
    public DbSet<CommentDataModel> Comments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoDataModel>()
                    .HasOne(todo => todo.OwnerUser)
                    .WithMany()
                    .HasForeignKey(todo => todo.OwnerUserId)
                    .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CommentDataModel>()
                    .HasOne(comment => comment.Todo)
                    .WithMany(todo => todo.Comments)
                    .HasForeignKey(comment => comment.TodoId)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CommentDataModel>()
                    .HasOne(comment => comment.OwnerUser)
                    .WithMany()
                    .HasForeignKey(comment => comment.OwnerUserId)
                    .OnDelete(DeleteBehavior.SetNull);
    }
}
