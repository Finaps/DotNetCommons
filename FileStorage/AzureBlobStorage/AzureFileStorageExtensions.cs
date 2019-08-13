using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Finaps.Commons.FileStorage.AzureFileStorage
{
  public static class AzureFileStorageExtensions
  {
    public static IServiceCollection ConfigureAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
      services.Configure<AzureBlobStorageOptions>(configuration.GetSection("AzureStorage"));
      services.AddScoped<IFileStorage>(sp =>
      {
        var options = sp.GetService<IOptions<AzureBlobStorageOptions>>();
        return new AzureFileStorage(options.Value);
      });
      return services;
    }
  }
}
