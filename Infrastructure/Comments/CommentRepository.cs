using Domain.Comments;
using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.DataModels;
using Infrastructure.Shared.Repository;

namespace Infrastructure.Comments;

public class CommentRepository : RepositoryBase<Comment, CommentDataModel>
{
    public CommentRepository(DataContext context) : base(context)
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

    protected override CommentDataModel ToDataModel(Comment item)
    {
        return new CommentDataModel()
        {
            Content = item.Content.Value,
            TodoId = item.TodoId,
            CreatedDateTime = item.CreatedDateTime,
            UpdatedDateTime = item.UpdatedDateTime,
            OwnerUserId = item.OwnerUserId
        };
    }

    protected override CommentDataModel Transfer
        (Comment item, CommentDataModel data)
    {
        data.Content = item.Content.Value;
        data.TodoId = item.TodoId;
        data.CreatedDateTime = item.CreatedDateTime;
        data.UpdatedDateTime = item.UpdatedDateTime;
        data.OwnerUserId = item.OwnerUserId;

        return data;
    }

    protected override Comment ToModel(CommentDataModel data)
    {
        return new(
            id: data.Id,
            content: new(data.Content),
            todoId: data.TodoId,
            createdDateTime: data.CreatedDateTime,
            updatedDateTime: data.UpdatedDateTime,
            ownerUserId: data?.OwnerUserId
        );
    }
}
