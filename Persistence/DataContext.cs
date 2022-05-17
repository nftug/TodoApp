using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain;
using Persistence.DataModels;

namespace Persistence;

public class DataContext : IdentityDbContext<ApplicationUser>
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
                    .HasOne(todoDataModel => todoDataModel.OwnerUser)
                    .WithMany()
                    .HasForeignKey(todoDataModel => todoDataModel.OwnerUserId)
                    .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CommentDataModel>()
                    .HasOne(comment => comment.Todo)
                    .WithMany(todo => todo.Comments)
                    .HasForeignKey(comment => comment.TodoId)
                    .OnDelete(DeleteBehavior.Cascade);
    }
}
