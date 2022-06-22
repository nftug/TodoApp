using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Todo;

public class TodoState : ValueObject<TodoState>
{
    public int Value { get; }

    public static readonly TodoState Todo = new(0);
    public static readonly TodoState Doing = new(1);
    public static readonly TodoState Done = new(2);

    public TodoState(int? value)
    {
        Value = value != null ? (int)value : Todo.Value;
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

public class TodoStateAttribute : ValidationAttribute
{
    public const int MaxStateValue = 2;

    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        int? state = (int?)value;
        string[] memberNames = new[] { validationContext.MemberName! };

        if (state < 0 || state > MaxStateValue)
            return new ValidationResult($"0-{MaxStateValue}の間で指定してください。", memberNames);

        return ValidationResult.Success;
    }
}