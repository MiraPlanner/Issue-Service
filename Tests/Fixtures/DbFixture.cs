using Mongo2Go;
using MongoDB.Driver;

namespace Tests;

[Trait("Category", "Integration")]
public class DbFixture : IDisposable
{
    public IMongoDatabase Database => _fakeDb;

    private MongoDbRunner _runner;
    private MongoClient _client;
    private IMongoDatabase _fakeDb;

    public DbFixture()
    {
        _runner = MongoDbRunner.Start();
        _client = new MongoClient(_runner.ConnectionString);
        _fakeDb = _client.GetDatabase("issues-test");
    }

    public void Dispose()
    {
        _runner.Dispose();
        _runner = null;
        _client = null;
        _fakeDb = null;
    }
}
