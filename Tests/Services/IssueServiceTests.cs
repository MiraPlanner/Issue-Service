using Issue_Service.Dtos;
using Issue_Service.Models;
using Issue_Service.Services;
using MassTransit;
using Mira_Common;
using Sprint_Service.Models;

namespace Tests;

public class IssueServiceTests
{
    private readonly Mock<IRepository<Issue>> _mockRepository;
    private readonly Mock<IPublishEndpoint> _mockPublishEndpoint;

    public IssueServiceTests()
    {
        _mockRepository = new Mock<IRepository<Issue>>();
        _mockPublishEndpoint = new Mock<IPublishEndpoint>();
    }
    
   [Fact]
   public void GetAll_ReturnsAllIssues()
   {
       // Arrange
       _mockRepository.Setup(repo => repo.GetAll())
           .ReturnsAsync(GetIssues());
       var service = new IssueService(_mockRepository.Object, _mockPublishEndpoint.Object);

       // Act
       var result = service.GetAll();

       // Assert
       var task = Assert.IsType<Task<IEnumerable<IssueDto>>>(result);
       var returnValue = task.Result;
       var issue = returnValue.FirstOrDefault();
       Assert.Equal("First Issue", issue!.Title);
   }

   [Fact]
   public void GetById_ReturnsIssue()
   {
       // Arrange
       Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
       _mockRepository.Setup(repo => repo.Get(testSessionGuid))
           .ReturnsAsync(GetIssue(testSessionGuid));
       var service = new IssueService(_mockRepository.Object, _mockPublishEndpoint.Object);

       // Act
       var result = service.GetById(testSessionGuid);

       // Assert
       var task = Assert.IsType<Task<IssueDto>>(result);
       var issue = Assert.IsType<IssueDto>(task.Result);
       Assert.Equal("Issue", issue.Title);
   }
   
   [Fact]
   public void GetById_ReturnsNull_WhenIssueNotFound()
   {
       // Arrange
       Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

       _mockRepository.Setup(repo => repo.Get(testSessionGuid))
           .ReturnsAsync((Issue) null!);
       var service = new IssueService(_mockRepository.Object, _mockPublishEndpoint.Object);

       // Act
       var result = service.GetById(testSessionGuid);

       // Assert
       var task = Assert.IsType<Task<IssueDto>>(result);
       Assert.Null(task.Result);
   }
   
   [Fact]
   public void Create_ReturnsIssue()
   {
       // Arrange
       var service = new IssueService(_mockRepository.Object, _mockPublishEndpoint.Object);
       CreateIssueDto createIssueDto = new CreateIssueDto(null, "New Issue", null, 5, IssueStatus.ToDo, IssueType.Task);
       
       // Act
       var result = service.Create(createIssueDto);

       // Assert
       var task = Assert.IsType<Task<IssueDto>>(result);
       var issue = task.Result;
       Assert.Equal("New Issue", issue.Title);
   }
   
   [Fact]
   public void Update_ReturnsUpdatedIssue()
   {
       // Arrange
       Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
       _mockRepository.Setup(repo => repo.Get(testSessionGuid))
           .ReturnsAsync(GetIssue(testSessionGuid));
       var service = new IssueService(_mockRepository.Object, _mockPublishEndpoint.Object);
       UpdateIssueDto updateIssueDto = new UpdateIssueDto(null, "Updated Issue", null, 5, IssueStatus.ToDo, IssueType.Task);
       
       // Act
       var result = service.Update(testSessionGuid, updateIssueDto);

       // Assert
       var task = Assert.IsType<Task<IssueDto>>(result);
       var updatedIssue = task.Result;
       Assert.Equal("Updated Issue", updatedIssue.Title);
   }

   [Fact]
   public void Update_ReturnsNull_WhenIssueNotFound()
   {
       // Arrange
       Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
       _mockRepository.Setup(repo => repo.Get(testSessionGuid))
           .ReturnsAsync((Issue) null!);
       var service = new IssueService(_mockRepository.Object, _mockPublishEndpoint.Object);
       UpdateIssueDto updateIssueDto = new UpdateIssueDto(null, "Updated Issue", null, 5, IssueStatus.ToDo, IssueType.Task);

       // Act
       var result = service.Update(testSessionGuid, updateIssueDto);

       // Assert
       var task = Assert.IsType<Task<IssueDto>>(result);
       Assert.Null(task.Result);
   }
   
   [Fact] 
   public void Delete_ReturnsDeletedIssue()
   {
       // Arrange
       Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
       _mockRepository.Setup(repo => repo.Get(testSessionGuid))
           .ReturnsAsync(GetIssue(testSessionGuid));
       var service = new IssueService(_mockRepository.Object, _mockPublishEndpoint.Object);
       
       // Act
       var result = service.Delete(testSessionGuid);

       // Assert
       var task = Assert.IsType<Task<IssueDto>>(result);
       var deletedIssue = task.Result;
       Assert.Equal("Very Complex", deletedIssue.Description);
   }
   
   [Fact] 
   public void Delete_ReturnsNull_WhenIssueNotFound()
   {
       // Arrange
       Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
       _mockRepository.Setup(repo => repo.Get(testSessionGuid))
           .ReturnsAsync((Issue) null!);
       var service = new IssueService(_mockRepository.Object, _mockPublishEndpoint.Object);
       
       // Act
       var result = service.Delete(testSessionGuid);

       // Assert
       var task = Assert.IsType<Task<IssueDto>>(result);
       Assert.Null(task.Result);
   }
   
   private static Issue GetIssue(Guid id)
   {
       return new Issue
       {
           Id = id,
           Title = "Issue",
           Description = "Very Complex",
           Duration = 50,
           IssueStatus = IssueStatus.InProgress,
           IssueType = IssueType.Task
       };
   }

   private static IReadOnlyCollection<Issue> GetIssues()
   {
       List<Issue> issues = new List<Issue>
       {
           new Issue()
           {
               Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"),
               Title = "First Issue",
               Description = "First Description",
               Duration = 10,
               IssueStatus = IssueStatus.ToDo,
               IssueType = IssueType.Task

           },
           new Issue()
           {
               Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0456"),
               Title = "Second Issue",
               Description = "Second Description",
               Duration = 20,
               IssueStatus = IssueStatus.InProgress,
               IssueType = IssueType.Subtask

           }
       };

       return issues;
   }
}