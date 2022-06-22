using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Comment;

public class CommentContent : ValueObject<CommentContent>
{
    public string Value { get; }

    public CommentContent(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(CommentContent other) => Value == other.Value;
}

public class CommentContentAttribute : ValidationAttribute
{
    public const int MaxContentLength = 140;

    protected override ValidationResult? IsValid(
        object? value, ValidationContext validationContext)
    {
        string? content = value as string;
        string[] memberNames = new[] { validationContext.MemberName! };

        if (string.IsNullOrWhiteSpace(content))
            return new ValidationResult("内容を入力してください。", memberNames);
        if (content.Length > MaxContentLength)
            return new ValidationResult($"{MaxContentLength}文字以内で入力してください。", memberNames);

        return ValidationResult.Success;
    }
}