using System;
using MongoDB.Driver;

namespace Finaps.Commons.MongoDB
{
  public class MongoConnection
  {
    public MongoClient client;
    public IMongoDatabase database;
    public MongoConnection()
    {
      Connect();
    }

    private void Connect()
    {
      string connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
      string databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");

      client = new MongoClient(connectionString);
      database = client.GetDatabase(databaseName);
    }
  }
}
