using System.ComponentModel.DataAnnotations;
using Domain.Shared.Exceptions;
using Domain.Shared.ValueObjects;

namespace Domain.Users.ValueObjects;

public class UserEmail : ValueObject<UserEmail>
{
    public string Value { get; }

    public UserEmail(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(nameof(UserEmail), "メールアドレスを入力してください。");
        if (!new EmailAddressAttribute().IsValid(value))
            throw new DomainException(nameof(UserEmail), "正しいメールアドレスを入力してください。");

        Value = value;
    }

    protected override bool EqualsCore(UserEmail other) => Value == other.Value;
}

public class UserEmailAttribute : ValueObjectAttributeBase<UserEmail, string?>
{
    protected override UserEmail CreateValueObject(string? value) => new(value);
}
