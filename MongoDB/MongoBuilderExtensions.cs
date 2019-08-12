using Microsoft.Extensions.DependencyInjection;

namespace Finaps.Commons.MongoDB
{
  public static class MongoBuilderExtensions
  {
    public static IServiceCollection AddMongoDBConnection(this IServiceCollection services) =>
        services.AddSingleton<MongoConnection>();


  }
}
