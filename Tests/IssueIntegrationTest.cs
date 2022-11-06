using System.Net.Http.Json;
using Issue_Service.Dtos;
using Sprint_Service.Models;

namespace Tests;

public class IssueIntegrationTest : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    public IssueIntegrationTest(CustomWebAppFactory<Program> factory) 
        => _client = factory.CreateClient();
    
    [Fact]
    public async Task Index_WhenCalled_ReturnsApplicationForm()
    {
        CreateIssueDto createIssueDto = new CreateIssueDto(
            new Guid(),
            "title",
            "description",
            0,
            IssueStatus.ToDo,
            IssueType.Task
        );
        
        var create = await _client.PostAsJsonAsync("/issues", createIssueDto);
        
        var response = await _client.GetAsync($"/issues/{new Guid()}");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Null(responseString);
    }
}