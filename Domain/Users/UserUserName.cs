using Domain.Shared;

namespace Domain.Users;

public class UserUserName : ValueObject<UserUserName, string>
{
    public const int MaxUserNameLength = 30;

    public UserUserName(string value) : base(value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("ユーザー名を入力してください");
        if (value.Length > MaxUserNameLength)
            throw new DomainException($"{MaxUserNameLength}文字以内で入力してください");
    }

    protected override bool EqualsCore(UserUserName other) => Value == other.Value;

    private DomainException CreateUserNameException(string message)
    {
        return new DomainException("username", message);
    }
}
