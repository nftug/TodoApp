using Domain.Shared;

namespace Domain.Todos;

public class TodoPeriod : ValueObject<TodoPeriod>
{
    public DateTime? BeginDateTimeValue { get; }
    public DateTime? DueDateTimeValue { get; }

    public TodoPeriod(DateTime? beginDateTimeValue, DateTime? dueDateTimeValue)
    {
        if (beginDateTimeValue > dueDateTimeValue)
            throw CreatePeriodException("開始日時が終了日時よりも前になるように指定してください");

        BeginDateTimeValue = beginDateTimeValue;
        DueDateTimeValue = dueDateTimeValue;
    }

    protected override bool EqualsCore(TodoPeriod other) =>
        (BeginDateTimeValue == other.BeginDateTimeValue) &&
        (DueDateTimeValue == other.DueDateTimeValue);

    private DomainException CreatePeriodException(string message)
    {
        return new DomainException("period", message);
    }

}
