using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.DataModels;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure;

public class DataContext : IdentityDbContext<UserDataModel<Guid>, IdentityRole<Guid>, Guid>
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<TodoDataModel> Todo { get; set; } = null!;
    public DbSet<CommentDataModel> Comment { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TodoDataModel>()
                    .ToTable("Todo");

        modelBuilder.Entity<CommentDataModel>()
                    .ToTable("Comment");

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
