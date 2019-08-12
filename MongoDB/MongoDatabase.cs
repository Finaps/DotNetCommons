using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Finaps.Commons.MongoDB
{
  public class MongoDatabase<T> : IDatabase<T> where T : IMongoModel
  {
    private readonly MongoClient client;
    private readonly IMongoDatabase database;
    private readonly IMongoCollection<T> collection;
    private readonly MongoConnection connection;
    public MongoDatabase(MongoConnection connection, string collectionName)
    {
      this.connection = connection;
      database = connection.database;
      client = connection.client;
      collection = database.GetCollection<T>(collectionName);
    }

    public T InsertItem(T obj, CancellationToken cancellationToken = default(CancellationToken))
    {
      collection.InsertOne(obj, cancellationToken: cancellationToken);
      return obj;
    }

    public void RemoveItem(string id, CancellationToken cancellationToken = default(CancellationToken))
    {
      var toRemove = new Guid(id);
      RemoveItem(toRemove, cancellationToken);
    }

    public void RemoveItem(T obj, CancellationToken cancellationToken = default(CancellationToken))
    {
      RemoveItem(obj.Id, cancellationToken);
    }

    public void RemoveItem(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
      collection.DeleteOne(GetId(id), cancellationToken: cancellationToken);
    }

    public T RetrieveItem(string id, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindSync(GetId(id), cancellationToken: cancellationToken).FirstOrDefault();
    }

    public T RetrieveItem(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindSync(GetId(id), cancellationToken: cancellationToken).FirstOrDefault();
    }

    public T RetrieveItem(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindSync(predicate, cancellationToken: cancellationToken).FirstOrDefault();
    }
    //important to cast to list to avoid cursor issues whilst quering db.
    public IList<T> RetrieveItems(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindSync(predicate, cancellationToken: cancellationToken).ToList();
    }

    public IList<T> RetrieveItems()
    {
      return collection.AsQueryable().ToList();
    }

    public IList<T> RetrieveItems(Expression<Func<T, bool>> predicate, int limit, int offset, CancellationToken cancellationToken = default(CancellationToken))
    {
      var options = CreateOptions(limit, offset);
      return collection.FindSync(predicate, options, cancellationToken).ToList();
    }

    public IList<T> RetrieveItems(int limit, int offset)
    {
      var options = CreateOptions(limit, offset);
      return collection.FindSync(new BsonDocument(), options).ToList();
    }

    public T UpdateItem(T obj, string id, CancellationToken cancellationToken = default(CancellationToken))
    {
      obj.Id = new Guid(id);
      return UpdateItem(obj, cancellationToken);
    }

    public T UpdateItem(T obj, CancellationToken cancellationToken = default(CancellationToken))
    {
      if (obj.Id == default(Guid))
        return default(T);
      var options = new FindOneAndReplaceOptions<T> { ReturnDocument = ReturnDocument.After };
      return collection.FindOneAndReplace(GetId(obj.Id), obj, options, cancellationToken);
    }

    public long Count(CancellationToken cancellationToken = default(CancellationToken))
    {
      return Count(new BsonDocument(), cancellationToken);
    }

    public long Count(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
      var options = Builders<T>.Filter.Where(predicate);
      return Count(options, cancellationToken);
    }

    public long Count(FilterDefinition<T> options, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.CountDocuments(options, cancellationToken: cancellationToken);
    }

    public async Task<T> InsertItemAsync(T obj, CancellationToken cancellationToken = default(CancellationToken))
    {
      var insertOptions = new InsertOneOptions();
      await collection.InsertOneAsync(obj, insertOptions, cancellationToken);
      return obj;
    }

    public Task<T> RetrieveItemAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindAsync(GetId(id), cancellationToken: cancellationToken).Result.FirstAsync(cancellationToken);
    }

    public Task<T> RetrieveItemAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindAsync(GetId(id), cancellationToken: cancellationToken).Result.FirstAsync(cancellationToken);
    }

    public Task<List<T>> RetrieveItemsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindAsync(predicate, cancellationToken: cancellationToken).Result.ToListAsync(cancellationToken);
    }

    public Task<List<T>> RetrieveItemsAsync(Expression<Func<T, bool>> predicate, int limit, int offset, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindAsync(predicate, CreateOptions(limit, offset), cancellationToken).Result.ToListAsync(cancellationToken);
    }

    public Task<List<T>> RetrieveItemsAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.AsQueryable().ToListAsync();
    }

    public Task<List<T>> RetrieveItemsAsync(int limit, int offset, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.FindAsync(new BsonDocument(), CreateOptions(limit, offset), cancellationToken).Result.ToListAsync();
    }

    public Task<T> UpdateItemAsync(T obj, string id, CancellationToken cancellationToken = default(CancellationToken))
    {
      obj.Id = new Guid(id);
      return UpdateItemAsync(obj, cancellationToken);
    }
    public Task<T> UpdateItemAsync(T obj, Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
      obj.Id = id;
      return UpdateItemAsync(obj, cancellationToken);
    }

    public Task<T> UpdateItemAsync(T obj, CancellationToken cancellationToken = default(CancellationToken))
    {
      if (obj.Id == default(Guid))
        return Task.FromResult(default(T));
      var options = new FindOneAndReplaceOptions<T> { ReturnDocument = ReturnDocument.After };
      return collection.FindOneAndReplaceAsync(GetId(obj.Id), obj, options, cancellationToken);
    }

    public Task<long> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
      return CountAsync(new BsonDocument(), cancellationToken);
    }

    public Task<long> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
    {
      var options = Builders<T>.Filter.Where(predicate);
      return CountAsync(options, cancellationToken);
    }

    public Task<long> CountAsync(FilterDefinition<T> options, CancellationToken cancellationToken = default(CancellationToken))
    {
      return collection.CountDocumentsAsync(options, cancellationToken: cancellationToken);
    }

    public async Task RemoveItemAsync(T obj, CancellationToken cancellationToken = default(CancellationToken))
    {
      await RemoveItemAsync(obj.Id);
    }

    public async Task RemoveItemAsync(string id, CancellationToken cancellationToken = default(CancellationToken))
    {
      var idToRemove = new Guid(id);
      await RemoveItemAsync(idToRemove, cancellationToken: cancellationToken);
    }

    public async Task RemoveItemAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
    {
      await collection.DeleteOneAsync(GetId(id), cancellationToken: cancellationToken);
    }
    private FindOptions<T> CreateOptions(int limit, int offset)
    {
      return new FindOptions<T>
      {
        Limit = limit,
        Skip = offset,
      };
    }

    private FilterDefinition<T> GetId(string id)
    {
      return GetId(new Guid(id));
    }

    private FilterDefinition<T> GetId(Guid id)
    {
      return Builders<T>.Filter.Eq(x => x.Id, id);
    }

    public IMongoQueryable<T> AsQueryable() => collection.AsQueryable();

  }
}
