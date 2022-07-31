using Domain.Comments.ValueObjects;
using Domain.Shared.Entities;
using Domain.Todos.Entities;

namespace Domain.Comments.Entities;

public class Comment : ModelBase
{
    public CommentContent Content { get; private set; } = null!;
    public Guid TodoId { get; private set; }
    public Todo Todo { get; set; } = null!;

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

    private Comment() { }

    public static Comment CreateNew(
        CommentContent content,
        Guid todoId,
        Guid ownerUserId
    )
    {
        var operationDateTime = DateTime.Now;

        return new()
        {
            Content = content,
            TodoId = todoId,
            CreatedOn = operationDateTime,
            UpdatedOn = operationDateTime,
            OwnerUserId = ownerUserId
        };
    }

    public void Edit(CommentContent content)
    {
        Content = content;
        UpdatedOn = DateTime.Now;
    }
}