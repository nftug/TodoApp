using Domain.Shared.Exceptions;
using Domain.Shared.ValueObjects;

namespace Domain.Users.ValueObjects;

public class UserName : ValueObject<UserName>
{
    public const int MaxUserNameLength = 30;

    public string Value { get; }

    public UserName(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(nameof(UserName), "ユーザー名を入力してください。");
        if (value.Length > MaxUserNameLength)
            throw new DomainException(nameof(UserName), $"{MaxUserNameLength}文字以内で入力してください。");

        Value = value;
    }

    protected override bool EqualsCore(UserName other) => Value == other.Value;
}

public class UserNameAttribute : ValueObjectAttributeBase<UserName, string?>
{
    protected override UserName CreateValueObject(string? value) => new(value);
}