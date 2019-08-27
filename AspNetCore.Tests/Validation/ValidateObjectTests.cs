using System;
using System.ComponentModel.DataAnnotations;
using AspNetCore.Tests.Validation;
using Xunit;

namespace AspNetCore.Tests
{
  public class ValidateObjectTests
  {
    [Fact]
    public void ValidationFailsForInvalidNestedClass()
    {
      var obj = ValidateObjectTestClass.InvalidNestedClass();
      var context = new ValidationContext(obj);
      Assert.Throws<ValidationException>(() => Validator.ValidateObject(obj, context, true));
    }

    [Fact]
    public void ValidationPassesForNoNestedClass()
    {
      var obj = ValidateObjectTestClass.NoNestedClass();
      var context = new ValidationContext(obj);
      Validator.ValidateObject(obj, context, true);
    }

    [Fact]
    public void ValidationPassesForValidNestedClass()
    {
      var obj = ValidateObjectTestClass.ValidNestedClass();
      var context = new ValidationContext(obj);
      Validator.ValidateObject(obj, context, true);
    }
  }
}
