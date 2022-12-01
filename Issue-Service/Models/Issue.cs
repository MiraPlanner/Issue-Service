using Mira_Common;
using Issue_Service.Dtos;
using Sprint_Service.Models;

namespace Issue_Service.Models;

public class Issue : IEntity
{
    public Guid Id { get; set; }
    public Guid? SprintId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public IssueStatus IssueStatus { get; set; }
    public IssueType IssueType { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; set; }

    public IssueDto AsDto()
    {
        return new IssueDto(
            Id, SprintId, Title, Description, Duration, IssueStatus, IssueType, CreatedAt, UpdatedAt
        );
    }
}