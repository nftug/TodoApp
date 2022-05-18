using Domain.Shared;

namespace Domain.Todos;

public class TodoTitle : ValueObject<TodoTitle>
{
    public string Value { get; }

    public const int MaxTitleLength = 50;

    public TodoTitle(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw CreateTitleException("タイトルを入力してください");

        if (value.Length > MaxTitleLength)
            throw CreateTitleException($"{MaxTitleLength}文字以内で入力してください");

        Value = value;
    }

    protected override bool EqualsCore(TodoTitle other) => Value == other.Value;

    private DomainException CreateTitleException(string message)
    {
        return new DomainException("title", message);
    }
}