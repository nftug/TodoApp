using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.User;

public class UserName : ValueObject<UserName>
{
    public string Value { get; }

    public UserName(string value)
    {
        Value = value;
    }

    protected override bool EqualsCore(UserName other) => Value == other.Value;
}

public class UserNameAttribute : ValidationAttribute
{
    public const int MaxUserNameLength = 30;

    protected override ValidationResult? IsValid
        (object? value, ValidationContext validationContext)
    {
        string? username = value as string;
        string[] memberNames = new[] { validationContext.MemberName! };

        if (string.IsNullOrWhiteSpace(username))
            return new ValidationResult("ユーザー名を入力してください。", memberNames);
        if (username.Length > MaxUserNameLength)
            return new ValidationResult($"{MaxUserNameLength}文字以内で入力してください", memberNames);

        return ValidationResult.Success;
    }
}