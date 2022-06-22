using AutoMapper;
using Domain.Comments;
using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.Shared.Repository;

namespace Infrastructure.Comments;

public class CommentRepository : RepositoryBase<Comment>
{
    public CommentRepository
        (DataContext context, IMapper mapper, IDataSource<Comment> source)
        : base(context, mapper, source)
    {
    }

    public override async Task<Comment> CreateAsync(Comment item)
    {
        // 外部キーの存在チェック
        var todoDataModel = await _context.Todos.FindAsync(item.TodoId);
        if (todoDataModel == null)
            throw new DomainException(nameof(item.TodoId), "このIDのTodoは存在しません");

        return await base.CreateAsync(item);
    }
}
