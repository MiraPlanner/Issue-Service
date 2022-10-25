using Sprint_Service.Models;

namespace Issue_Service.Dtos;

public record IssueDto(
    Guid Id, 
    Guid? SprintId,
    string Title, 
    string? Description, 
    int? Duration, 
    IssueStatus IssueStatus,
    IssueType IssueType,
    DateTimeOffset CreatedAt, 
    DateTimeOffset UpdatedAt
);
