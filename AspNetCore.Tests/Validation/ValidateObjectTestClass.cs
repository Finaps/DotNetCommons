using System.ComponentModel.DataAnnotations;
using Finaps.Commons.AspNetCore.Validation;

namespace AspNetCore.Tests.Validation
{
  public class ValidateObjectTestClass
  {
    [ValidateObject]
    public ValidateObjectNestedClass NestedClass { get; set; }


    public class ValidateObjectNestedClass
    {
      [Required]
      public string RequiredString { get; set; }
    }

    public static ValidateObjectTestClass NoNestedClass()
    {
      return new ValidateObjectTestClass();
    }

    public static ValidateObjectTestClass InvalidNestedClass()
    {
      return new ValidateObjectTestClass()
      {
        NestedClass = new ValidateObjectNestedClass()
      };
    }

    public static ValidateObjectTestClass ValidNestedClass()
    {
      return new ValidateObjectTestClass()
      {
        NestedClass = new ValidateObjectNestedClass()
        {
          RequiredString = "hello"
        }
      };
    }
  }
}