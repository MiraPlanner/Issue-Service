using System.Net;
using System.Net.Http.Json;
using Issue_Service.Dtos;
using Sprint_Service.Models;
using Tests.Fixtures;

namespace Tests.Controller;

public class IssueControllerIntegrationTests : IClassFixture<DbFixture>, IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public IssueControllerIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_ReturnsNewlyCreatedIssue()
    {
        // Arrange
        CreateIssueDto createIssueDto = new CreateIssueDto(
            Guid.NewGuid(),
            "new issue",
            "lots of work to do here!",
            0,
            IssueStatus.ToDo,
            IssueType.Task
        );
        
        // Act
        var response = await _client.PostAsJsonAsync("/issues", createIssueDto);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var responseObject = await response.Content.ReadFromJsonAsync<IssueDto>();
        
        Assert.NotNull(responseObject);
        Assert.IsType<Guid>(responseObject!.Id);
        Assert.IsType<Guid>(responseObject.SprintId);
        Assert.Equal("new issue", responseObject.Title);
        Assert.Equal("lots of work to do here!", responseObject.Description);
        Assert.Equal(0, responseObject.Duration);
        Assert.Equal(IssueStatus.ToDo, responseObject.IssueStatus);
        Assert.Equal(IssueType.Task, responseObject.IssueType);
        Assert.Equal(DateTime.Now, responseObject.CreatedAt.DateTime, TimeSpan.FromMinutes(1));
        Assert.Equal(DateTime.Now, responseObject.UpdatedAt.DateTime, TimeSpan.FromMinutes(1));
    }
    
    [Fact]
    public async Task Post_ReturnsBadRequest_WhenIssueInvalid()
    {
        // Arrange
        CreateIssueDto createIssueDto = new CreateIssueDto(
            Guid.NewGuid(),
            "",
            "desc",
            0,
            IssueStatus.ToDo,
            IssueType.Task
        );
        
        // Act
        var response = await _client.PostAsJsonAsync("/issues", createIssueDto);
        
        // Assert
        Assert.IsType<HttpResponseMessage>(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"title\":\"One or more validation errors occurred.\"", responseString);
    }
}