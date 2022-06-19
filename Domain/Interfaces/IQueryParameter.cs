namespace Domain.Interfaces;

public interface IQueryParameter<T>
    where T : IEntity
{
    int Page { get; init; }
    int Limit { get; init; }
}
