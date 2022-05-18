using Domain.Shared;

namespace Domain.Todos;

public class TodoTitle : ValueObject<TodoTitle, string>
{
    public const int MaxTitleLength = 50;

    public TodoTitle(string value) : base(value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw CreateTitleException("タイトルを入力してください");

        if (value.Length > MaxTitleLength)
            throw CreateTitleException($"{MaxTitleLength}文字以内で入力してください");
    }

    protected override bool EqualsCore(TodoTitle other) => Value == other.Value;

    private DomainException CreateTitleException(string message)
    {
        return new DomainException("title", message);
    }
}