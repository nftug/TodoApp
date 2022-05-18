using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain;
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
                    .HasOne(todoDataModel => todoDataModel.OwnerUser)
                    .WithMany()
                    .HasForeignKey(todoDataModel => todoDataModel.OwnerUserId)
                    .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<CommentDataModel>()
                    .HasOne(commentDataModel => commentDataModel.Todo)
                    .WithMany(todoDataModel => todoDataModel.Comments)
                    .HasForeignKey(commentDataModel => commentDataModel.TodoId)
                    .OnDelete(DeleteBehavior.Cascade);
    }
}
