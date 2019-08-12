using System;
using System.Collections.Generic;

using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Finaps.Commons.MongoDB
{
  public interface IDatabase<T>
  {
    IMongoQueryable<T> AsQueryable();
    T InsertItem(T obj, CancellationToken cancellationToken = default(CancellationToken));
    Task<T> InsertItemAsync(T obj, CancellationToken cancellationToken = default(CancellationToken));
    T RetrieveItem(string id, CancellationToken cancellationToken = default(CancellationToken));
    T RetrieveItem(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task<T> RetrieveItemAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
    Task<T> RetrieveItemAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    IList<T> RetrieveItems(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    IList<T> RetrieveItems(Expression<Func<T, bool>> predicate, int limit, int offset, CancellationToken cancellationToken = default(CancellationToken));
    IList<T> RetrieveItems();
    IList<T> RetrieveItems(int limit, int offset);
    Task<List<T>> RetrieveItemsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    Task<List<T>> RetrieveItemsAsync(Expression<Func<T, bool>> predicate, int limit, int offset, CancellationToken cancellationToken = default(CancellationToken));
    Task<List<T>> RetrieveItemsAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<List<T>> RetrieveItemsAsync(int limit, int offset, CancellationToken cancellationToken = default(CancellationToken));
    T UpdateItem(T obj, string id, CancellationToken cancellationToken = default(CancellationToken));
    T UpdateItem(T obj, CancellationToken cancellationToken = default(CancellationToken));
    Task<T> UpdateItemAsync(T obj, string id, CancellationToken cancellationToken = default(CancellationToken));
    Task<T> UpdateItemAsync(T obj, Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task<T> UpdateItemAsync(T obj, CancellationToken cancellationToken = default(CancellationToken));
    long Count(CancellationToken cancellationToken = default(CancellationToken));
    long Count(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    long Count(FilterDefinition<T> options, CancellationToken cancellationToken = default(CancellationToken));
    Task<long> CountAsync(CancellationToken cancellationToken = default(CancellationToken));
    Task<long> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
    Task<long> CountAsync(FilterDefinition<T> options, CancellationToken cancellationToken = default(CancellationToken));
    void RemoveItem(string id, CancellationToken cancellationToken = default(CancellationToken));
    void RemoveItem(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    Task RemoveItemAsync(T obj, CancellationToken cancellationToken = default(CancellationToken));
    Task RemoveItemAsync(string id, CancellationToken cancellationToken = default(CancellationToken));
    Task RemoveItemAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));
    void RemoveItem(T obj, CancellationToken cancellationToken = default(CancellationToken));
  }
}
