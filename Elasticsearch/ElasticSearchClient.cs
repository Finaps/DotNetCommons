using System;
using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Nest;
using Polly;

namespace Finaps.Commons.ElasticSearch
{
  public class ElasticSearchClient : IElasticSearchClient
  {
    private IElasticClient _client;
    private readonly ILogger _logger;

    public IElasticClient Client => _client;

    public ElasticSearchClient(ILogger<ElasticSearchClient> logger, string url)
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
      CheckConnection();
      _logger.LogInformation($"Successfully created connection to Elastic Search");
    }

    private void CheckConnection()
    {
      var policy = Polly.Policy.HandleResult<PingResponse>(r => !r.IsValid)
              .WaitAndRetry(
                5,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (result, timeSpan, context) =>
                {
                  _logger.LogError(result.Result.OriginalException, $"Error connecting to ElasticSearch");
                }
              );
      var response = policy.Execute(() => _client.Ping());
      if (!response.IsValid)
      {
        throw new ElasticSearchException($"Error connecting to ElasticSearch", response.OriginalException);
      }
    }

    public ElasticSearchResponse AddIndex<T>(string indexName = null) where T : class
    {
      indexName = indexName ?? GetTypeIndexName(typeof(T));
      var existsResponse = _client.Indices.Exists(indexName);
      if (!existsResponse.IsValid)
      {
        return ConstructResponse(existsResponse, $"Error checking if index {indexName} already exists\n");
      }
      if (existsResponse.Exists)
      {
        _logger.LogDebug($"Index {indexName} already exists, overriding mapping");
        var mapResponse = _client.Map<T>(m => m.Index(indexName).AutoMap());
        return ConstructResponse(mapResponse, $"Error overriding mapping on index {indexName}\n");
      }
      else
      {
        _logger.LogDebug($"Created index {indexName} for type {typeof(T).Name}");
        var createIndexResponse = _client.Indices.Create(indexName, c => c
            .Map<T>(x => x
                .AutoMap<T>()
            )
        );
        return ConstructResponse(createIndexResponse, $"Error creating index {indexName}\n");
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

    public ElasticSearchResponse Delete<T>(string id, string indexName = null) where T : class
    {
      indexName = indexName ?? GetTypeIndexName(typeof(T));
      _logger.LogDebug("Removing document from index [" + indexName + "] with Id [" + id + "]");
      var deleteResponse = _client.Delete<T>(id, s => s
          .Index(indexName)
      );
      return ConstructResponse(deleteResponse, $"Error removing document from index [{indexName}] with id {id}\n");
    }

    public ElasticSearchResponse Put<T>(T obj, string indexName = null) where T : class
    {
      indexName = indexName ?? GetTypeIndexName(typeof(T));
      _logger.LogTrace($"Indexing object for index [{indexName}]\n");
      var fluentIndexResponse = _client.Index(obj, i => i.Index(indexName));
      return ConstructResponse(fluentIndexResponse, $"Error indexing object for index [{indexName}]\n");
    }

    private ElasticSearchResponse ConstructResponse(ResponseBase nestResponse, string errorPrefix = "")
    {
      if (nestResponse.IsValid)
      {
        return ElasticSearchResponse.SuccessResponse();
      }
      else
      {
        string error = ConstructErrorCause(nestResponse.ServerError.Error);
        return ElasticSearchResponse.ErrorResponse(error);
      }
    }

    private string ConstructErrorCause(Error error, string errorPrefix = "")
    {
      return $"{errorPrefix}Reason: {error.Reason}\nCausedBy: {error.CausedBy}\n RootCause: {error.RootCause}";
    }
  }
}
