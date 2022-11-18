

using Castle.Core.Configuration;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Mira_Common.Settings;
using MongoDB.Driver;

namespace Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbFixture _dbFixture;
    
    public CustomWebApplicationFactory(DbFixture dbFixture)
    {
        _dbFixture = dbFixture;
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(IMongoDatabase));
            if (descriptor != null)
                services.Remove(descriptor);
            services.AddSingleton(_ => _dbFixture.Database);
            
            services.RemoveMassTransitHostedService();
            services.AddMassTransitTestHarness();
        });
    }
}