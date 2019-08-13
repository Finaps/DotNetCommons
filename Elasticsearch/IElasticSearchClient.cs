using System;
using Nest;

namespace Finaps.Commons.Elasticsearch
{
  public interface IElasticsearchClient
  {
    void AddIndex<T>(string indexName = null) where T : class;
    bool Put<T>(T obj, string indexName = null) where T : class;
    bool Delete<T>(string id, string indexName = null) where T : class;
    ISearchResponse<T> Search<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query, string indexName = null) where T : class;
    IElasticClient Client { get; }
  }
}
