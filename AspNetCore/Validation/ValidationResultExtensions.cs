using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Finaps.Commons.AspNetCore.Validation
{
  public static class ValidationResultExtensions
  {
    public static string GenerateErrorMessage(this ValidationResult validationResult)
    {
      string message = validationResult.ErrorMessage;
      if (validationResult is CompositeValidationResult)
      {
        var compositeValidationResult = validationResult as CompositeValidationResult;
        message += ":\n" + string.Join("\n", compositeValidationResult.Results.Select(result => result.ErrorMessage));
      }
      return message;
    }
  }
}
