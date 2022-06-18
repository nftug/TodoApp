namespace Domain.Shared;

public interface IQueryParameter
{
    int Page { get; set; }
    int Limit { get; set; }
}
