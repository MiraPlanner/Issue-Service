using Issue_Service.Controllers;
using Issue_Service.Dtos;
using Issue_Service.Interfaces;
using Issue_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Sprint_Service.Models;

namespace Tests;

public class IssueControllerTests
{
    private readonly Mock<IIssueService> _mockService;
    
    public IssueControllerTests()
    {
        _mockService = new Mock<IIssueService>();
    }
    
    [Fact]
    public void GetById_ReturnsAnIssue()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

        _mockService.Setup(service => service.GetById(testSessionGuid))
            .ReturnsAsync(GetIssueDto(testSessionGuid));
        var controller = new IssueController(_mockService.Object);

        // Act
        var result = controller.GetById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IssueDto>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var issue = Assert.IsType<IssueDto>(okResult.Value);
        Assert.Equal("Title", issue.Title);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenIssueNotFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        var mockService = new Mock<IIssueService>();
        mockService.Setup(service => service.GetById(testSessionGuid))
            .ReturnsAsync((IssueDto) null!);
        var controller = new IssueController(mockService.Object);

        // Act
        var result = controller.GetById(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IssueDto>>>(result);
        var actionResult = task.Result;
        Assert.IsType<NotFoundResult>(actionResult.Result);
    }

    [Fact]
    public void GetAll_ReturnsAllIssues()
    {
        _mockService.Setup(service => service.GetAll())
            .ReturnsAsync(GetIssueDtos());
        var controller = new IssueController(_mockService.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<IssueDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<IssueDto>>(okResult.Value);
        var issue = returnValue.FirstOrDefault();
        Assert.Equal("First Issue", issue!.Title);
    }

    [Fact]
    public void GetAll_ReturnsNoIssues_WhenIssuesNotFound()
    {
        _mockService.Setup(service => service.GetAll())
            .ReturnsAsync(new List<IssueDto>());
        var controller = new IssueController(_mockService.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<IssueDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<IssueDto>>(okResult.Value);
        Assert.Empty(returnValue);
    }
    
    private IssueDto GetIssueDto(Guid id)
    {
        return new Issue()
        {
            Id = id,
            Title = "Title",
            Description = "Description",
            Duration = 5,
            IssueStatus = IssueStatus.ToDo,
            IssueType = IssueType.UserStory
        }.AsDto();
    }

    private IEnumerable<IssueDto> GetIssueDtos()
    {
        List<IssueDto> issueDtos = new List<IssueDto>();
        issueDtos.Add(new Issue()
        {
            Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"),
            Title = "First Issue",
            Description = "First Description",
            Duration = 10,
            IssueStatus = IssueStatus.ToDo,
            IssueType = IssueType.Task

        }.AsDto());
        
        issueDtos.Add(new Issue()
        {
            Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"),
            Title = "Second Issue",
            Description = "Second Description",
            Duration = 20,
            IssueStatus = IssueStatus.InProgress,
            IssueType = IssueType.Subtask

        }.AsDto());
        
        return issueDtos;
    }
}