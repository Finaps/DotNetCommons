using System;
using System.Collections.Generic;
using System.Linq;

namespace Finaps.Commons
{
  public static class StringExtensions
  {
    public static IEnumerable<Guid> ParseListOfGuids(this string guidString)
    {
      return guidString.Split(',').Select(x => Guid.Parse(x));
    }
  }
}