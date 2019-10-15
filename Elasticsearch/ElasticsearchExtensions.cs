using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Finaps.Commons.ElasticSearch.DependencyInjection
{
  public static class ElasticSearchExtensions
  {

    public static void AddElasticsearch(
        this IServiceCollection services, string url)
    {
      services.AddSingleton<IElasticSearchClient>(sp =>
      {
        var logger = sp.GetRequiredService<ILogger<ElasticSearchClient>>();
        return new ElasticSearchClient(logger, url);
      });
    }

    public static void AddElasticsearchIndex<T>(this IApplicationBuilder app, string indexName = null) where T : class
    {
      var client = app.ApplicationServices.GetRequiredService<IElasticSearchClient>();
      client.AddIndex<T>();
    }
  }
}
