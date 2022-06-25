using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Todo;

public class TodoDescription : ValueObject<TodoDescription>
{
    public string? Value { get; }

    public TodoDescription(string? value)
    {
        Value = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    protected override bool EqualsCore(TodoDescription other) => Value == other.Value;
}

public class TodoDescriptionAttribute : ValidationAttribute
{
    public const int MaxDescriptionLength = 140;

    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        string? description = value as string;
        string[] memberNames = new[] { validationContext.MemberName! };

        if (description?.Length > MaxDescriptionLength)
            return new ValidationResult($"{MaxDescriptionLength}文字以内で入力してください。", memberNames);

        return ValidationResult.Success;
    }
}