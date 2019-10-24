namespace Finaps.Commons.ElasticSearch
{
  [System.Serializable]
  public class ElasticSearchException : System.Exception
  {
    public ElasticSearchException() { }
    public ElasticSearchException(string message) : base(message) { }
    public ElasticSearchException(string message, System.Exception inner) : base(message, inner) { }
    protected ElasticSearchException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}