using Domain.Shared;
using System.ComponentModel.DataAnnotations;

namespace Domain.Todos;

public class TodoTitle : ValueObject<TodoTitle>
{
    public string Value { get; }

    public TodoTitle(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(TodoTitle other) => Value == other.Value;
}

public class TodoTitleAttribute : ValidationAttribute
{
    public const int MaxTitleLength = 50;

    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        string? title = value as string;
        string[] memberNames = new[] { validationContext.MemberName! };

        if (string.IsNullOrWhiteSpace(title))
            return new ValidationResult("タイトルを入力してください。", memberNames);
        if (title.Length > MaxTitleLength)
            return new ValidationResult($"{MaxTitleLength}文字以内で入力してください。", memberNames);

        return ValidationResult.Success;
    }
}