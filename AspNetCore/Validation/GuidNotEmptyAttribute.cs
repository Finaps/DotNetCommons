using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Validation
{
    public class GuidNotEmptyAttribute : ValidationAttribute
    {
           protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
              if (string.IsNullOrEmpty(ErrorMessage))
              {
                ErrorMessage = "Guid cannot be empty";
              }
              if (!(value is Guid))
              {
                throw new InvalidOperationException($"{nameof(GuidNotEmptyAttribute)} can only be used on properties of type Guid");
              }
        
              return (Guid)value == Guid.Empty ? new ValidationResult(ErrorMessage) : ValidationResult.Success;
            }
    }
}