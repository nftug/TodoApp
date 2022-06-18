using Domain.Shared;

namespace Domain.Interfaces;

public interface IQueryParameter<T>
    where T : IEntity
{
    int Page { get; set; }
    int Limit { get; set; }
}
