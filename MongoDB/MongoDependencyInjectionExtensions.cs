using System;
using Microsoft.Extensions.DependencyInjection;

namespace Finaps.Commons.MongoDB
{
  public static class MongoDependencyInjectionExtensions
  {
    public static IServiceCollection AddMongoDBConnection(this IServiceCollection services) =>
        services.AddSingleton<MongoConnection>();
    [Obsolete]
    private static IServiceCollection ConfigureMongo<T>(this IServiceCollection services) where T : IMongoModel
    {
      return services.AddSingleton<IDatabase<T>, MongoDatabase<T>>((ctx) =>
      {
        MongoConnection connection = ctx.GetRequiredService<MongoConnection>();
        return new MongoDatabase<T>(connection, typeof(T).Name);
      });
    }
  }

}
