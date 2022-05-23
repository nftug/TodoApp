using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Users;

public class UserEmail : ValueObject<UserEmail>
{
    public string Value { get; }

    public UserEmail(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(UserEmail other) => Value == other.Value;
}

public class UserEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        string? email = value as string;
        string[] memberNames = new[] { validationContext.MemberName! };

        if (string.IsNullOrWhiteSpace(email))
            return new ValidationResult("メールアドレスを入力してください。", memberNames);
        if (!new EmailAddressAttribute().IsValid(email))
            return new ValidationResult("正しいメールアドレスを入力してください。", memberNames);

        return ValidationResult.Success;
    }
}
