using Mira_Common;
using Issue_Service.Dtos;

namespace Issue_Service.Models;

public class Issue : IEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }
    public IssueType IssueType { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public IssueDto AsDto()
    {
        return new IssueDto(
            Id, Title, Description, Duration, IssueType, CreatedAt, UpdatedAt
        );
    }
}