using Domain.Shared.Exceptions;
using Domain.Shared.ValueObjects;

namespace Domain.Comments.ValueObjects;

public class CommentContent : ValueObject<CommentContent>
{
    public const int MaxContentLength = 140;

    public string Value { get; }

    public CommentContent(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(nameof(CommentContent), "内容を入力してください。");
        if (value.Length > MaxContentLength)
            throw new DomainException(nameof(CommentContent), $"{MaxContentLength}文字以内で入力してください。");

        Value = value;
    }

    protected override bool EqualsCore(CommentContent other) => Value == other.Value;
}

public class CommentContentAttribute : ValueObjectAttributeBase<CommentContent, string?>
{
    public CommentContentAttribute(bool isPatch = false) : base(isPatch) { }
    protected override CommentContent CreateValueObject(string? value) => new(value);
}