using Domain.Shared;

namespace Domain.Todos;

public class TodoState : ValueObject<TodoState>
{
    public int Value { get; set; }

    public const int MaxStateValue = 2;

    public static readonly TodoState Todo = new TodoState(0);
    public static readonly TodoState Doing = new TodoState(1);
    public static readonly TodoState Done = new TodoState(2);

    public TodoState(int value)
    {
        if (value < 0 || value > MaxStateValue)
            throw CreateStateException($"0-{MaxStateValue}の間で指定してください");

        Value = value;
    }

    protected override bool EqualsCore(TodoState other) => Value == other.Value;

    public string DisplayValue
    {
        get
        {
            if (this == Doing) return "DOING";
            if (this == Done) return "DONE";
            else return "TODO";
        }
    }

    private DomainException CreateStateException(string message)
    {
        return new DomainException("state", message);
    }
}
