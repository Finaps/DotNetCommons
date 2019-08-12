using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Finaps.Commons.MongoDB
{
  public interface IMongoModel
  {
    [BsonId]
    Guid Id { get; set; }
  }
}
