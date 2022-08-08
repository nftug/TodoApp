using System.ComponentModel.DataAnnotations;
using Domain.Shared.Exceptions;

namespace Domain.Shared.ValueObjects;

public abstract class ValueObjectAttributeBase<T, TValue> : ValidationAttribute
    where T : ValueObject<T>
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        TValue? _value = (TValue?)value;
        string[] memberNames = new[] { validationContext.MemberName! };

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