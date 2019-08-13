using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finaps.Commons.AspNetCore.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Finaps.Commons.MongoDB
{
  public static class MongoExtensions
  {
    private static async Task<PaginatedResponse<T>> AsPaginatedResponse<T>(IMongoQueryable<T> workflows, int limit, int offset)
    {
      var data = new List<T>();
      if (limit > 0)
      {
        data = await workflows.Skip(offset).Take(limit).ToListAsync();
      }
      var count = await workflows.CountAsync();
      return PaginatedResponse<T>.Create(data, limit, offset, count);
    }
  }
}
