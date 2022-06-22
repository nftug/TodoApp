using Domain.Comments;
using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Comments;

public class CommentRepository : RepositoryBase<Comment>
{
    public CommentRepository
        (DataContext context, IDataSource<Comment> source)
        : base(context, source)
    {
    }

    public override async Task<Comment> CreateAsync(Comment item)
    {
        // 外部キーの存在チェック
        var existsParent = await _context.Todos
            .AnyAsync(x => x.Id == item.TodoId);
        if (!existsParent)
            throw new DomainException
                (nameof(item.TodoId), "このIDのTodoは存在しません");

        return await base.CreateAsync(item);
    }
}
