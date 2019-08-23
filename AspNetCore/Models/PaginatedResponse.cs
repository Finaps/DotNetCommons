using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finaps.Commons.AspNetCore.Extensions
{
  public class PaginatedResponse<T>
  {
    public long Total { get; set; }
    public int Limit { get; set; }
    public int Offset { get; set; }
    public IList<T> Data { get; set; }
    public static PaginatedResponse<T> Create(IList<T> data, int limit, int offset, long total)
    {
      return new PaginatedResponse<T>()
      {
        Limit = limit,
        Offset = offset,
        Total = total,
        Data = data
      };
    }

    public async static Task<PaginatedResponse<T>> CreateFromAsync<FT>(PaginatedResponse<FT> original, Func<FT, Task<T>> mapper)
    {
      var data = await Task.WhenAll(original.Data.Select(mapper));
      return Create(data, original.Limit, original.Offset, original.Total);
    }

    public static PaginatedResponse<T> CreateFrom<FT>(PaginatedResponse<FT> original, Func<FT, T> mapper)
    {
      var data = original.Data.Select(mapper).ToList();
      return Create(data, original.Limit, original.Offset, original.Total);
    }
  }
}
