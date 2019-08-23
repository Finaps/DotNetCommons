using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finaps.Commons.AspNetCore.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Finaps.Commons.MongoDB
{
  public static class MongoUtilities
  {
    public static async Task<PaginatedResponse<T>> AsPaginatedResponse<T>(this IMongoQueryable<T> workflows, int limit, int offset)
    {
      var data = new List<T>();
      if (limit > 0)
      {
        data = await workflows.Skip(offset).Take(limit).ToListAsync();
      }
      var count = await workflows.CountAsync();
      return PaginatedResponse<T>.Create(data, limit, offset, count);
    }

    public static FilterDefinition<T> IdFilter<T>(T obj) where T : IMongoModel
    {
      return Builders<T>.Filter.Eq(x => x.Id, obj.Id);
    }

    public static FilterDefinition<T> IdFilter<T>(Guid id) where T : IMongoModel
    {
      return Builders<T>.Filter.Eq(x => x.Id, id);
    }
  }
}
