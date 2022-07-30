using Domain.Shared.Exceptions;
using Domain.Shared.ValueObjects;

namespace Domain.Todos.ValueObjects;

public class TodoDescription : ValueObject<TodoDescription>
{
    public const int MaxDescriptionLength = 140;

    public string? Value { get; }

    public TodoDescription(string? value)
    {
        if (value?.Length > MaxDescriptionLength)
            throw new DomainException(nameof(TodoDescription), $"{MaxDescriptionLength}文字以内で入力してください。");

        Value = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    protected override bool EqualsCore(TodoDescription other) => Value == other.Value;
}

public class TodoDescriptionAttribute : ValueObjectAttributeBase<TodoDescription, string?>
{
    protected override TodoDescription CreateValueObject(string? value) => new(value);
}
