using System.ComponentModel.DataAnnotations;
using Domain.Shared.Exceptions;

namespace Domain.Shared.ValueObjects;

public abstract class ValueObjectAttributeBase<T, TValue> : ValidationAttribute
    where T : ValueObject<T>
{
    protected bool IsPatch { get; } = false;

    protected ValueObjectAttributeBase(bool isPatch = false)
    {
        IsPatch = isPatch;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        TValue? _value = (TValue?)value;
        string[] memberNames = new[] { validationContext.MemberName! };

        if (IsPatch && _value == null) return ValidationResult.Success;

        try
        {
            var item = CreateValueObject(_value);
            return ValidationResult.Success;
        }
        catch (DomainException e)
        {
            return new ValidationResult(e.Message, memberNames);
        }
    }

    protected abstract T CreateValueObject(TValue? value);
}