using System;
using Nest;

namespace Finaps.Commons.ElasticSearch
{
  public interface IElasticSearchClient
  {
    ElasticSearchResponse AddIndex<T>(string indexName = null) where T : class;
    ElasticSearchResponse Put<T>(T obj, string indexName = null) where T : class;
    ElasticSearchResponse Delete<T>(string id, string indexName = null) where T : class;
    ISearchResponse<T> Search<T>(Func<QueryContainerDescriptor<T>, QueryContainer> query, string indexName = null) where T : class;
    IElasticClient Client { get; }
  }
}
