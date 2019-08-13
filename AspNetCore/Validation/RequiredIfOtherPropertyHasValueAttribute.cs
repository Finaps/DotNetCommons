using System;
using System.ComponentModel.DataAnnotations;

namespace Finaps.Commons.AspNetCore.Validation
{
  public class RequiredIfOtherPropertyHasValueAttribute : ValidationAttribute
  {
    private readonly string propertyName;
    private readonly object propertyValueToCheck;

    public RequiredIfOtherPropertyHasValueAttribute(
      string propertyName,
      object propertyValueToCheck
    )
    {
      this.propertyName = propertyName;
      this.propertyValueToCheck = propertyValueToCheck;
    }

    override protected ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var property = validationContext.ObjectType.GetProperty(propertyName);
      if (property == null)
      {
        throw new InvalidOperationException($"Property {propertyName} does not exist on type {validationContext.ObjectType.FullName}");
      }
      var propertyValue = property.GetValue(validationContext.ObjectInstance, null);
      if (propertyValue.Equals(propertyValueToCheck))
      {
        if (value == null)
        {
          return new ValidationResult(ErrorMessage);
        }
      }

      return ValidationResult.Success;
    }
  }
}
