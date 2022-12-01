using Mongo2Go;
using MongoDB.Driver;

namespace Tests.Fixtures;

[Trait("Category", "Integration")]
public class DbFixture : IDisposable
{
    public IMongoDatabase Database { get; }
    private readonly MongoDbRunner _runner;
    private readonly string _dbName = "Issue";
    
    public DbFixture()
    {
        _runner = MongoDbRunner.Start();
        var client = new MongoClient(_runner.ConnectionString);
        Database = client.GetDatabase(_dbName);
    }

    public void Dispose()
    {
        _runner.Dispose();
    }
}
