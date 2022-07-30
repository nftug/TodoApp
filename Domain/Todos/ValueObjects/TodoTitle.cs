using Domain.Shared.Exceptions;
using Domain.Shared.ValueObjects;

namespace Domain.Todos.ValueObjects;

public class TodoTitle : ValueObject<TodoTitle>
{
    public const int MaxTitleLength = 50;

    public string Value { get; }

    public TodoTitle(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(nameof(TodoTitle), "タイトルを入力してください。");
        if (value.Length > MaxTitleLength)
            throw new DomainException(nameof(TodoTitle), $"{MaxTitleLength}文字以内で入力してください。");

        Value = value;
    }

    protected override bool EqualsCore(TodoTitle other) => Value == other.Value;
}

public class TodoTitleAttribute : ValueObjectAttributeBase<TodoTitle, string?>
{
    protected override TodoTitle CreateValueObject(string? value) => new(value);
}