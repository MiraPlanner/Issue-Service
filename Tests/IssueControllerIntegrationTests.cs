using Issue_Service.Controllers;
using Issue_Service.Dtos;
using Issue_Service.Models;
using Issue_Service.Services;
using MassTransit;
using Mira_Common.MongoDB;
using Sprint_Service.Models;

namespace Tests;

public class IssueControllerIntegrationTests : IClassFixture<DbFixture>
{
    private MongoRepository<Issue> repository;
    private IssueController _issueController;

    public IssueControllerIntegrationTests(DbFixture dbFixture)
    {
        var mockPublishEndpoint = new Mock<IPublishEndpoint>();

        repository = new MongoRepository<Issue>(dbFixture.Database, "issues-test");
        var service = new IssueService(repository, mockPublishEndpoint.Object);
       _issueController = new IssueController(service);
    }
    
    [Fact]
    public async Task Find_Null_When_No_Entry_With_Id_Found()
    {
        var actual = await repository.Get(new Guid());
        Assert.Null(actual);
    }
    
    [Fact]
    public async void CreateItem()
    {
        Issue issue = new Issue()
        {
            SprintId = new Guid(),
            Title = "title",
            Description = "description",
            Duration = 0,
            IssueStatus = IssueStatus.ToDo,
            IssueType = IssueType.Task,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };

        CreateIssueDto createIssueDto = new CreateIssueDto(
            new Guid(),
            "title",
            "description",
            0,
            IssueStatus.ToDo,
            IssueType.Task
        );
        
        
        await _issueController.Create(createIssueDto);
        var issues = await repository.GetAll();
        
        Assert.Null(issues);
    }
}