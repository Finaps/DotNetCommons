using System;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;

namespace Finaps.Commons.Elasticsearch
{
  public class ElasticsearchClient : IElasticsearchClient
  {
    private IElasticClient _client;
    private readonly ILogger _logger;

    public IElasticClient Client => _client;

    public ElasticsearchClient(ILogger<ElasticsearchClient> logger, string url)
    {
      _logger = logger;
      InitializeClient(url);
    }

    private void InitializeClient(string url)
    {
      _logger.LogInformation($"Connecting to elastic search on {url}");
      var pool = new SingleNodeConnectionPool(new Uri(url));
      var settings = new ConnectionSettings(pool);
      _client = new ElasticClient(settings);
      _logger.LogInformation($"Successfully created connection to Elastic Search");
    }
    public void AddIndex<T>(string indexName = null) where T : class
    {
      indexName = indexName ?? GetTypeIndexName(typeof(T));
      if (!_client.Indices.Exists(indexName).Exists)
      {
        _client.Indices.Create(indexName, c => c
            .Map<T>(x => x
                .AutoMap<T>()
            )
        );
        _logger.LogInformation($"Created index {indexName} for type {typeof(T).Name}");
      }
      else
      {
        _client.Map<T>(m => m.Index(indexName).AutoMap());
        _logger.LogInformation($"Index {indexName} already exists, overriding mapping");
      }
    }

    public ISearchResponse<T> Search<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query, string indexName = null) where T : class
    {
      indexName = indexName ?? GetTypeIndexName(typeof(T));
      return _client.Search<T>(s => s.Index(indexName).Query(query));
    }

    private string GetTypeIndexName(Type t)
    {
      return t.Name.ToLowerInvariant();
    }

    public bool Delete<T>(string id, string indexName = null) where T : class
    {
      indexName = indexName ?? GetTypeIndexName(typeof(T));
      _logger.LogDebug("Removing document from index [" + indexName + "] with Id [" + id + "]");
      var deleteResponse = _client.Delete<T>(id, s => s
          .Index(indexName)
      );
      if (deleteResponse.IsValid) return true;
      _logger.LogWarning("Error removing object with Id " + id + " from index: " + deleteResponse.Result);
      return false;
    }

    public bool Put<T>(T obj, string indexName = null) where T : class
    {
      indexName = indexName ?? GetTypeIndexName(typeof(T));
      var fluentIndexResponse = _client.Index(obj, i => i.Index(indexName));
      if (fluentIndexResponse.IsValid) return true;
      _logger.LogWarning("Error writing object to index: " + fluentIndexResponse.Result);
      return false;
    }
  }
}
