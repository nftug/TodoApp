using System.ComponentModel.DataAnnotations;
using Domain.Shared;

namespace Domain.Users;

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

        if (string.IsNullOrWhiteSpace(username))
            return new ValidationResult("ユーザー名を入力してください。");
        if (username.Length > MaxUserNameLength)
            return new ValidationResult($"{MaxUserNameLength}文字以内で入力してください");

        return ValidationResult.Success;
    }
}