using Domain.Shared;

namespace Domain.User;

public class UserService : DomainServiceBase<UserModel>
{
    public override Task<bool> CanDelete(UserModel item, Guid? userId)
        => Task.FromResult(item.Id == userId);

    public override Task<bool> CanEdit(UserModel item, Guid? userId)
        => Task.FromResult(item.Id == userId);
}
