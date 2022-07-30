using Domain.Interfaces;
using Domain.Shared.Services;
using Domain.Users.Entities;

namespace Domain.Users.Services;

public class UserService : DomainServiceBase<User>
{
    public UserService(IRepository<User> repository) : base(repository)
    {
    }

    public override Task<bool> CanDelete(User item, Guid? userId)
        => Task.FromResult(item.Id == userId);

    public override Task<bool> CanEdit(User item, Guid? userId)
        => Task.FromResult(item.Id == userId);
}
