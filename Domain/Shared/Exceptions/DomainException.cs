namespace Domain.Shared.Exceptions;

public class DomainException : Exception
{
    public Dictionary<string, List<string>> Errors { get; } = new() { };

    public DomainException(string field, string message)
        : base(message)
    {
        Errors[field] = new List<string> { message };
    }

    public DomainException(string message)
        : this("other", message)
    {
    }

    public DomainException(Dictionary<string, List<string>> errors)
    {
        Errors = errors;
    }

    public DomainException() { }

    public void Add(string field, string message)
    {
        if (!Errors.ContainsKey(field)) Errors[field] = new List<string> { };
        Errors[field].Add(message);
    }
}
