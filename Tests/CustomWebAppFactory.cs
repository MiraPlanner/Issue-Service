

using Castle.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Mira_Common.Settings;
using MongoDB.Driver;

namespace Tests;

public class CustomWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    private readonly IntegrationDb _integrationDb;
    
    public CustomWebAppFactory()
    {
        _integrationDb = new IntegrationDb();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(MongoDB.Driver.IMongoDatabase));
            if (descriptor != null)
                services.Remove(descriptor);
            services.AddSingleton(_ => _integrationDb.Database);
        });
    }
}