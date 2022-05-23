using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Todos;

public class TodoState : ValueObject<TodoState>
{
    public int Value { get; set; }

    public static readonly TodoState Todo = new TodoState(0);
    public static readonly TodoState Doing = new TodoState(1);
    public static readonly TodoState Done = new TodoState(2);

    public TodoState(int value)
    {
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
}

public class TodoStateAttribute : ValidationAttribute
{
    public const int MaxStateValue = 2;

    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        int? state = (int?)value;

        if (state < 0 || state > MaxStateValue)
            return new ValidationResult($"0-{MaxStateValue}の間で指定してください。");

        return ValidationResult.Success;
    }
}