using System.ComponentModel.DataAnnotations;

namespace Finaps.Commons.AspNetCore.Extensions
{
  public class Pagination
  {
    [Range(0, int.MaxValue)]
    public int Offset { get; set; }
    [Range(0, int.MaxValue)]
    public int Limit { get; set; } = 10;
  }

}
