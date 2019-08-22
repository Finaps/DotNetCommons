using MongoDB.Driver;

namespace Finaps.Commons.MongoDB
{
  public class MongoConnection
  {
    public MongoClient Client { get; private set; }
    public IMongoDatabase Database { get; private set; }
    public MongoConnection(string connectionString, string databaseName)
    {
      Client = new MongoClient(connectionString);
      Database = Client.GetDatabase(databaseName);
    }
  }
}
