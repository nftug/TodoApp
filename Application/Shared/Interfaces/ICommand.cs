using Domain.Shared.Entities;

namespace Application.Shared.Interfaces;

public interface ICommand<TDomain>
    where TDomain : ModelBase
{
    Guid? Id { get; set; }
}
