using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Finaps.Commons.Elasticsearch.DependencyInjection
{
  public static class ElasticsearchExtensions
  {

    public static void AddElasticsearch(
        this IServiceCollection services, string url)
    {
      services.AddSingleton<IElasticsearchClient>(sp =>
      {
        var logger = sp.GetRequiredService<ILogger<ElasticsearchClient>>();
        return new ElasticsearchClient(logger, url);
      });
    }

    public static void AddElasticsearchIndex<T>(this IApplicationBuilder app, string indexName = null) where T : class
    {
      var client = app.ApplicationServices.GetRequiredService<IElasticsearchClient>();
      client.AddIndex<T>();
    }
  }
}
