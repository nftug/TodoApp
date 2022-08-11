using Domain.Shared.Exceptions;
using Domain.Shared.Interfaces;
using Domain.Shared.Services;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users.Services;

public class UserService : DomainServiceBase<User>
{
    private readonly IUserRepository _userRepository;

    public UserService(IRepository<User> repository) : base(repository)
    {
        _userRepository = (IUserRepository)repository;
    }

    public override async Task<bool> CanCreate(User item, Guid? userId = null)
        => await CanEdit(item, userId);

    public override Task<bool> CanDelete(User item, Guid? userId)
        => Task.FromResult(item.Id == userId);

    public override async Task<bool> CanEdit(User item, Guid? userId)
    {
        var exception = new DomainException();

        var other = await _userRepository.FindByEmail(item.Email.Value);
        if (!(other == null || other.Id == userId))
            exception.Add(nameof(UserEmail), "メールアドレスが重複しています。");

        other = await _userRepository.FindByUserName(item.UserName.Value);
        if (!(other == null || other.Id == userId))
            exception.Add(nameof(UserName), "ユーザーIDが重複しています。");

        if (exception.Errors.Count > 1) throw exception;

        return item.Id == userId;
    }
}
