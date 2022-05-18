using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Users;

public class UserEmail : ValueObject<UserEmail>
{
    public string Value { get; }

    public UserEmail(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw CreateEmailException("メールアドレスを入力してください");
        if (!new EmailAddressAttribute().IsValid(value))
            throw CreateEmailException("正しいメールアドレスを入力してください");

        Value = value;
    }

    protected override bool EqualsCore(UserEmail other) => Value == other.Value;

    private DomainException CreateEmailException(string message)
    {
        return new DomainException("email", message);
    }
}
