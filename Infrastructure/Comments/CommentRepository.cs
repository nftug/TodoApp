using AutoMapper;
using Domain.Comments;
using Domain.Shared;
using Infrastructure.DataModels;
using Infrastructure.Shared.Repository;

namespace Infrastructure.Comments;

public class CommentRepository : RepositoryBase<Comment, CommentDataModel>
{
    public CommentRepository(DataContext context, IMapper mapper)
        : base(context, mapper)
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
