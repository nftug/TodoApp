using Domain.Shared.Exceptions;
using Domain.Shared.ValueObjects;

namespace Domain.Todos.ValueObjects;

public class TodoState : ValueObject<TodoState>
{
    public const int MaxStateValue = 2;

    public int Value { get; init; }

    public static readonly TodoState Todo = new(0);
    public static readonly TodoState Doing = new(1);
    public static readonly TodoState Done = new(2);

    public static readonly TodoState[] Status = new TodoState[] { Todo, Doing, Done };

    public TodoState(int? value)
    {
        if (value < 0 || value > MaxStateValue)
            throw new DomainException(nameof(TodoState), $"0-{MaxStateValue}の間で指定してください。");

        Value = value != null ? (int)value : Todo.Value;
    }

    public TodoState(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Value = Todo.Value;
        }
        else
        {
            var state = Status.FirstOrDefault(x => x.DisplayValue.ToLower() == value?.ToLower());
            if (state == null)
                throw new DomainException(nameof(TodoState), "不正な値です。");
            Value = state.Value;
        }
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
}

public class TodoStateAttribute : ValueObjectAttributeBase<TodoState, string?>
{
    public TodoStateAttribute(bool isPatch = false) : base(isPatch) { }
    protected override TodoState CreateValueObject(string? value)
        => new(value);
}