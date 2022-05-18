using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Users;

public class UserEmail : ValueObject<UserEmail, string>
{
    public UserEmail(string value) : base(value)
    {
        if (!new EmailAddressAttribute().IsValid(value))
            throw CreateEmailException("正しいメールアドレスを入力してください");
    }

    protected override bool EqualsCore(UserEmail other) => Value == other.Value;

    private DomainException CreateEmailException(string message)
    {
        return new DomainException("email", message);
    }
}
