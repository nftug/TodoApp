using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Todo;

public class TodoPeriod : ValueObject<TodoPeriod>
{
    public DateTime? StartDateValue { get; }
    public DateTime? EndDateValue { get; }

    public TodoPeriod(DateTime? startDate, DateTime? endDate)
    {
        StartDateValue = startDate;
        EndDateValue = endDate;
    }

    protected override bool EqualsCore(TodoPeriod other) =>
        (StartDateValue == other.StartDateValue) &&
        (EndDateValue == other.EndDateValue);
}

public class TodoPeriodAttribute : ValidationAttribute
{
    public string OtherProperty { get; private set; }
    public Period Period { get; private set; }

    public TodoPeriodAttribute(Period period, string otherProperty)
    {
        OtherProperty = otherProperty;
        Period = period;
    }

    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        var propertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);
        DateTime? other = propertyInfo?.GetValue(validationContext.ObjectInstance, null) as DateTime?;
        DateTime? dateTime = value as DateTime?;
        string[] memberNames = new[] { validationContext.MemberName! };

        if (Period == Period.Begin && dateTime > other)
            return new ValidationResult("終了日時よりも前になるように指定してください。", memberNames);
        if (Period == Period.Due && dateTime < other)
            return new ValidationResult("開始日時よりも後になるように指定してください。", memberNames);

        return ValidationResult.Success;
    }
}

public enum Period { Begin, Due }