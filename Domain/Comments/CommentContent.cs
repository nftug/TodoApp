using Domain.Shared;

namespace Domain.Comments;

public class CommentContent : ValueObject<CommentContent, string>
{
    public const int MaxTodoTitleLength = 140;

    public CommentContent(string value) : base(value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw CreateTitleException("内容を入力してください");

        if (value.Length > MaxTodoTitleLength)
            throw CreateTitleException($"{MaxTodoTitleLength}文字以内で入力してください");
    }

    protected override bool EqualsCore(CommentContent other) => Value == other.Value;

    private DomainException CreateTitleException(string message)
    {
        return new DomainException("content", message);
    }
}