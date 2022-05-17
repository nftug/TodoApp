using Domain.Shared;
using Domain.Comments;

namespace Domain.Todos;

public class Todo
{
    public Guid Id { get; set; }
    public TodoTitle Title { get; private set; }
    public TodoDescription? Description { get; private set; }
    public DateTime? BeginDateTime { get; private set; }
    public DateTime? DueDateTime { get; private set; }
    public TodoState State { get; private set; }
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    public DateTime CreatedDateTime { get; private set; }
    public DateTime UpdatedDateTime { get; private set; }
    public string? OwnerUserId { get; private set; }

    private Todo(
        Guid id,
        TodoTitle title,
        TodoDescription? description,
        DateTime? beginDateTime,
        DateTime? dueDateTime,
        TodoState state,
        ICollection<Comment> comments,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        string? ownerUserId
    )
    {
        Id = id;
        Title = title;
        Description = description;
        BeginDateTime = beginDateTime;
        DueDateTime = dueDateTime;
        State = state;
        Comments = comments;
        CreatedDateTime = createdDateTime;
        UpdatedDateTime = updatedDateTime;
        OwnerUserId = ownerUserId;
    }

    public static Todo CreateNew(
        TodoTitle title,
        TodoDescription? description,
        DateTime? beginDateTime,
        DateTime? dueDateTime,
        TodoState state,
        string ownerUserId
    )
    {
        var operationDateTime = DateTime.Now;

        ValidateBeginDueDateTime(beginDateTime, dueDateTime);

        return new Todo(
            id: new Guid(),  // Guidの生成はEF Coreに任せる
            title: title,
            description: description,
            beginDateTime: beginDateTime,
            dueDateTime: dueDateTime,
            state: state,
            comments: new List<Comment>(),
            createdDateTime: operationDateTime,
            updatedDateTime: operationDateTime,
            ownerUserId: ownerUserId
        );
    }

    public static Todo CreateFromRepository(
        Guid id,
        TodoTitle title,
        TodoDescription? description,
        DateTime? beginDateTime,
        DateTime? dueDateTime,
        TodoState state,
        ICollection<Comment> comments,
        DateTime createdDateTime,
        DateTime updatedDateTime,
        string? ownerUserId
    )
    {
        return new Todo(
            id: id,
            title: title,
            description: description,
            beginDateTime: beginDateTime,
            dueDateTime: dueDateTime,
            state: state,
            comments: comments,
            createdDateTime: createdDateTime,
            updatedDateTime: updatedDateTime,
            ownerUserId: ownerUserId
        );
    }

    public void Edit(
        TodoTitle title,
        TodoDescription? description,
        DateTime? beginDateTime,
        DateTime? dueDateTime,
        TodoState state
    )
    {
        ValidateBeginDueDateTime(beginDateTime, dueDateTime);

        Title = title;
        Description = description;
        BeginDateTime = beginDateTime;
        DueDateTime = dueDateTime;
        State = state;
        UpdatedDateTime = DateTime.Now;
    }

    private static void ValidateBeginDueDateTime(DateTime? beginDateTime, DateTime? dueDateTime)
    {
        if (beginDateTime > dueDateTime)
            throw new DomainException(
                "beginAndDueDateTime", "開始日時が終了日時よりも前になるように指定してください"
            );
    }
}
