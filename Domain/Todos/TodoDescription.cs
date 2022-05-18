using Domain.Shared;

namespace Domain.Todos;

public class TodoDescription : ValueObject<TodoDescription, string>
{
    public const int MaxDescriptionLength = 140;

    public TodoDescription(string value) : base(value)
    {
        if (value.Length > MaxDescriptionLength)
            throw CreateDescriptionException($"{MaxDescriptionLength}文字以内で入力してください");
    }

    protected override bool EqualsCore(TodoDescription other) => Value == other.Value;

    private DomainException CreateDescriptionException(string message)
    {
        return new DomainException("description", message);
    }
}
