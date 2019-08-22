using System;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Finaps.Commons.MongoDB
{
  public static class MongoDependencyInjectionExtensions
  {
    public static IServiceCollection AddMongoDBConnection(
      this IServiceCollection services,
      MongoOptions options = null)
    {
      options = options ?? new MongoOptions()
      {
        ConnectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING"),
        Database = Environment.GetEnvironmentVariable("DATABASE_NAME")
      };
      var connection = new MongoConnection(options.ConnectionString, options.Database);
      return services.AddSingleton<MongoConnection>(connection);
    }

    [Obsolete]
    private static IServiceCollection ConfigureMongo<T>(this IServiceCollection services) where T : IMongoModel
    {
      return services.AddSingleton<IDatabase<T>, MongoDatabase<T>>((ctx) =>
      {
        MongoConnection connection = ctx.GetRequiredService<MongoConnection>();
        return new MongoDatabase<T>(connection, typeof(T).Name);
      });
    }

    public static IServiceCollection UseMongoDBCollection<T>(this IServiceCollection services)
    {
      return services.AddScoped<IMongoCollection<T>>((ctx) =>
      {
        MongoConnection connection = ctx.GetRequiredService<MongoConnection>();
        return connection.Database.GetCollection<T>(typeof(T).Name);
      });
    }
  }

}
