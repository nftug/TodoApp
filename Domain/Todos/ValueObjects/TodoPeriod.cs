using System.ComponentModel.DataAnnotations;
using Domain.Shared.Exceptions;
using Domain.Shared.ValueObjects;

namespace Domain.Todos.ValueObjects;

public class TodoPeriod : ValueObject<TodoPeriod>
{
    public DateTime? StartDateValue { get; }
    public DateTime? EndDateValue { get; }

    public TodoPeriod(DateTime? startDate, DateTime? endDate)
    {
        if (startDate > endDate)
            throw new DomainException(nameof(TodoPeriod), "期間指定が不正です。");

        StartDateValue = startDate;
        EndDateValue = endDate;
    }

    protected override bool EqualsCore(TodoPeriod other) =>
        (StartDateValue == other.StartDateValue) &&
        (EndDateValue == other.EndDateValue);
}

public class TodoPeriodAttribute : ValidationAttribute
{
    private string OtherProperty { get; }
    private ArgumentType Type { get; }

    public TodoPeriodAttribute(ArgumentType type, string otherProperty)
    {
        Type = type;
        OtherProperty = otherProperty;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        DateTime? _value = (DateTime?)value;
        string[] memberNames = new[] { validationContext.MemberName! };

        var propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
        DateTime? otherValue = (DateTime?)propertyInfo?.GetValue(validationContext.ObjectInstance, null);

        try
        {
            var item = Type == ArgumentType.Start
                ? new TodoPeriod(_value, otherValue)
                : new TodoPeriod(otherValue, _value);

            return ValidationResult.Success;
        }
        catch (DomainException)
        {
            if (Type == ArgumentType.Start)
                return new ValidationResult("開始日は終了日よりも前に指定してください。", memberNames);
            else
                return new ValidationResult("終了日は開始日よりも後に指定してください。", memberNames);
        }
    }
}

public enum ArgumentType { Start, End }