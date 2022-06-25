using Domain.Comment;
using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.Services.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Comment;

public class CommentRepository : RepositoryBase<CommentModel>
{
    public CommentRepository
        (DataContext context, IDataSource<CommentModel> source)
        : base(context, source)
    {
    }

    public override async Task<CommentModel> CreateAsync(CommentModel item)
    {
        // 外部キーの存在チェック
        var existsParent = await _context.Todo
            .AnyAsync(x => x.Id == item.TodoId);
        if (!existsParent)
            throw new DomainException
                (nameof(item.TodoId), "このIDのTodoは存在しません");

        return await base.CreateAsync(item);
    }
}
