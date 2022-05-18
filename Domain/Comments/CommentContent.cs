using Domain.Shared;

namespace Domain.Comments;

public class CommentContent : ValueObject<CommentContent>
{
    public string Value { get; }

    public const int MaxContentLength = 140;

    public CommentContent(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw CreateTitleException("内容を入力してください");

        if (value.Length > MaxContentLength)
            throw CreateTitleException($"{MaxContentLength}文字以内で入力してください");

        Value = value;
    }

    protected override bool EqualsCore(CommentContent other) => Value == other.Value;

    private DomainException CreateTitleException(string message)
    {
        return new DomainException("content", message);
    }
}