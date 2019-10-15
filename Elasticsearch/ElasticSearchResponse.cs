namespace Finaps.Commons.ElasticSearch
{
  public class ElasticSearchResponse
  {
    public bool Success { get; set; }
    public string Error { get; set; }

    public static ElasticSearchResponse SuccessResponse()
    {
      return new ElasticSearchResponse()
      {
        Success = true
      };
    }

    public static ElasticSearchResponse ErrorResponse(string message)
    {
      return new ElasticSearchResponse()
      {
        Success = false,
        Error = message
      };
    }
  }


}