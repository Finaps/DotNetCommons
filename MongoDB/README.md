# Using mongo in an Asp.Net Core application

To configure the connection to MongoDB, call the following function in your `Startup.cs`

```csharp
services.AddMongoDBConnection();
```

It is possible to supply this function with a `MongoOptions` object specifying the connection string and database name to be used. If this object is not given the connection string and database name will be taken from the environment values CONNECTIONSTRING and DATABASE_NAME, respectively (This is for backward compatibility reasons).

To use a collection for a certain type in your application, add the following line to your `Startup.cs`:

```csharp
services.UseMongoDBCollection<T>();
```

Where T must implement `IMongoModel`. You can then use an `IMongoCollection<T>` as a dependency in your repository.

## Utilities

### Querying

It is possible to retrieve a `IMongoQueryable<T>` by running

```csharp
IMongoCollection<T> collection ...
IMongoQueryable<T> queryable = collection.AsQueryable();
```

This object allows you to retrieve objects from the database using Linq methods. For example:

```csharp
async Task<T> GetById(Guid id)
{
  return await queryable.SingleOrDefaultAsync(t => t.Id == id);
}
```

### IdFilter

A situation that often comes up when writing your repository is where you need to update or delete an existing object based on its id. The MongoCollection methods require you to specify a filter for this. A pre-made filter for finding an object based on its `Id` attribute can be retrieved by using the `MongoUtilities.IdFilter<T>()` function. Some examples:

```csharp
void Update(T obj)
{
  var idFilter = MongoUtilities.IdFilter(T obj);
  mongoCollection.ReplaceOne(idFilter, obj);
}

void Delete(Guid id)
{
  var idFilter = MongoUtilities.IdFilter<T>(id);
  mongoCollection.DeleteOne(idFilter);
}
```

Where T must implement `IMongoModel`.

### Paginated responses

If you want to return an `IMongoQueryable<T>` as a paginated object it is possible to use the `AsPaginatedResponse<T>` function:

```csharp
var filteredQuery = queryable.Where(t => ...);
PaginatedResponse<T> response = await filteredQuery.AsPaginatedResponse(limit, offset);
```

This will do a call to the database to retrieve the data (based on the limit and offset) and a total count.
