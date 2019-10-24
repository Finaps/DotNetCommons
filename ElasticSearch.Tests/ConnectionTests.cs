using System;
using Finaps.Commons.ElasticSearch;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ElasticSearch.Tests
{
  public class ConnectionTests
  {
    [Fact]
    public void CanConnect()
    {
      var logger = NullLogger<ElasticSearchClient>.Instance;
      var client = new ElasticSearchClient(logger, "http://localhost:9200");
    }
  }
}
