using Domain.Shared;

namespace Domain.Users;

public class UserUserName : ValueObject<UserUserName>
{
    public string Value { get; }

    public const int MaxUserNameLength = 30;

    public UserUserName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw CreateUserNameException("ユーザー名を入力してください");
        if (value.Length > MaxUserNameLength)
            throw CreateUserNameException($"{MaxUserNameLength}文字以内で入力してください");

        Value = value;
    }

    protected override bool EqualsCore(UserUserName other) => Value == other.Value;

    private DomainException CreateUserNameException(string message)
    {
        return new DomainException("username", message);
    }
}
