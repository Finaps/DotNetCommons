using Microsoft.Extensions.DependencyInjection;

namespace Finaps.Commons.MongoDB
{
  public static class MongoDependencyInjectionExtensions
  {
    public static IServiceCollection AddMongoDBConnection(this IServiceCollection services) =>
        services.AddSingleton<MongoConnection>();
  }
}
