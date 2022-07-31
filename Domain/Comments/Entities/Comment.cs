using Domain.Comments.ValueObjects;
using Domain.Shared.Entities;
using Domain.Todos.Entities;

namespace Domain.Comments.Entities;

public class Comment : ModelBase
{
    public CommentContent Content { get; private set; } = null!;
    public Guid TodoId { get; private init; }
    internal Todo Todo { get; } = null!;

    public Comment(
        Guid id,
        DateTime createdOn,
        DateTime updatedOn,
        Guid? ownerUserId,
        CommentContent content,
        Guid todoId,
        Todo todo
    )
        : base(id, createdOn, updatedOn, ownerUserId)
    {
        Content = content;
        TodoId = todoId;
        Todo = todo;
    }

    private Comment(Guid ownerUserId) : base(ownerUserId) { }

    public static Comment CreateNew(CommentContent content, Guid todoId, Guid ownerUserId)
        => new(ownerUserId)
        {
            Content = content,
            TodoId = todoId
        };

    public void Edit(CommentContent content)
    {
        Content = content;
        SetUpdatedOn();
    }
}
