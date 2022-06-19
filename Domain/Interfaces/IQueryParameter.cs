namespace Domain.Interfaces;

public interface IQueryParameter<TEntity>
    where TEntity : IEntity
{
    int Page { get; init; }
    int Limit { get; init; }
}
