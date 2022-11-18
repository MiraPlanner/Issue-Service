using Mongo2Go;
using MongoDB.Driver;

namespace Tests.Fixtures;

[Trait("Category", "Integration")]
public class DbFixture : IDisposable
{
    public IMongoDatabase Database => _fakeDb;

    private MongoDbRunner _runner;
    private MongoClient _client;
    private IMongoDatabase _fakeDb;
    private string _dbName = "Issue";
    
    public DbFixture()
    {
        _runner = MongoDbRunner.Start();
        _client = new MongoClient(_runner.ConnectionString);
        _fakeDb = _client.GetDatabase(_dbName);
    }

    public void Dispose()
    {
        _runner.Dispose();
        _runner = null;
        _client = null;
        _fakeDb = null;
    }
}
