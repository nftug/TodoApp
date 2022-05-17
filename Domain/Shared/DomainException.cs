namespace Domain.Shared;

public class DomainException : Exception
{
    public string Field { get; }

    public DomainException(string field, string message)
        : base(message)
    {
        Field = field;
    }

    public DomainException(string message)
        : base(message)
    {
        Field = "other";
    }
}
