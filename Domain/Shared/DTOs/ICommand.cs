using Domain.Shared.Entities;

namespace Domain.Shared.DTOs;

public interface ICommand<TDomain>
    where TDomain : ModelBase
{
    Guid? Id { get; set; }
}
