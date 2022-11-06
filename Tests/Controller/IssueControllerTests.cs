using Issue_Service.Controllers;
using Issue_Service.Dtos;
using Issue_Service.Interfaces;
using Issue_Service.Models;
using Microsoft.AspNetCore.Mvc;
using Sprint_Service.Models;

namespace Tests.Controller;

public class IssueControllerTests
{
    private readonly Mock<IIssueService> _mockService;
    
    public IssueControllerTests()
    {
        _mockService = new Mock<IIssueService>();
    }

    [Fact]
    public void GetAll_ReturnsAllIssues()
    {
        // Arrange
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
        // Arrange
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
    
    [Fact]
    public void GetById_ReturnsIssue()
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
    public void Create_ReturnsBadRequest_GivenInvalidIssue()
    {
        // Arrange
        var controller = new IssueController(_mockService.Object);
        CreateIssueDto createIssueDto = new CreateIssueDto(null, "", null, 5, IssueStatus.ToDo, IssueType.Task);
        
        // Act
        var result = controller.Create(createIssueDto);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IssueDto>>>(result);
        var actionResult = task.Result;
        Assert.IsType<BadRequestResult>(actionResult.Result);
    }

    [Fact] 
    public void Create_ReturnsNewlyCreatedIssue()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.Create(It.IsAny<CreateIssueDto>()))
            .ReturnsAsync(GetIssueDto(testSessionGuid));
        var controller = new IssueController(_mockService.Object);
        CreateIssueDto createIssueDto = new CreateIssueDto(null, "Title", "Description", 5, IssueStatus.ToDo, IssueType.UserStory);
        
        // Act
        var result = controller.Create(createIssueDto);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IssueDto>>>(result);
        var okResult = Assert.IsType<CreatedAtActionResult>(task.Result.Result);
        var issue = Assert.IsType<IssueDto>(okResult.Value);
        Assert.Equal("Title", issue.Title);
    }
    
    [Fact] 
    public void Update_ReturnsNoContent()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.Update(testSessionGuid, It.IsAny<UpdateIssueDto>()))
            .ReturnsAsync(GetIssueDto(testSessionGuid));
        var controller = new IssueController(_mockService.Object);
        UpdateIssueDto updateIssueDto = new UpdateIssueDto(null, "Title", "Description", 5, IssueStatus.ToDo, IssueType.UserStory);
        
        // Act
        var result = controller.Update(testSessionGuid, updateIssueDto);

        // Assert
        var task = Assert.IsType<Task<ActionResult>>(result);
        Assert.IsType<NoContentResult>(task.Result);
    }
    
    [Fact] 
    public void Update_ReturnsNotFound_WhenIssueNotFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        var mockService = new Mock<IIssueService>();
        mockService.Setup(service => service.Update(testSessionGuid, It.IsAny<UpdateIssueDto>()))
            .ReturnsAsync((IssueDto) null!);
        var controller = new IssueController(mockService.Object);
        UpdateIssueDto updateIssueDto = new UpdateIssueDto(null, "", null, 5, IssueStatus.ToDo, IssueType.Task);
        
        // Act
        var result = controller.Update(testSessionGuid, updateIssueDto);

        // Assert
        var task = Assert.IsType<Task<ActionResult>>(result);
        Assert.IsType<NotFoundResult>(task.Result);
    }
    
    [Fact] 
    public void Delete_ReturnsNoContent()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.Delete(testSessionGuid))
            .ReturnsAsync(GetIssueDto(testSessionGuid));
        var controller = new IssueController(_mockService.Object);
        
        // Act
        var result = controller.Delete(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult>>(result);
        Assert.IsType<NoContentResult>(task.Result);
    }
    
    [Fact] 
    public void Delete_ReturnsNotFound_WhenIssueNotFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
        var mockService = new Mock<IIssueService>();
        mockService.Setup(service => service.Delete(testSessionGuid))
            .ReturnsAsync((IssueDto) null!);
        var controller = new IssueController(mockService.Object);
        
        // Act
        var result = controller.Delete(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult>>(result);
        Assert.IsType<NotFoundResult>(task.Result);
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