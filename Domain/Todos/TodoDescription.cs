using Domain.Shared;

namespace Domain.Todos;

public class TodoDescription : ValueObject<TodoDescription>
{
    public string? Value { get; set; }

    public const int MaxDescriptionLength = 140;

    public TodoDescription(string? value)
    {
        if (value?.Length > MaxDescriptionLength)
            throw CreateDescriptionException($"{MaxDescriptionLength}文字以内で入力してください");

        Value = value;
    }

    protected override bool EqualsCore(TodoDescription other) => Value == other.Value;

    private DomainException CreateDescriptionException(string message)
    {
        return new DomainException("description", message);
    }
}
