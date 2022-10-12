using Issue_Service.Models;

namespace Issue_Service.Dtos;

public record IssueDto(
    Guid Id, 
    string Title, 
    string Description, 
    int Duration, 
    IssueType IssueType,
    DateTimeOffset CreatedAt, 
    DateTimeOffset UpdatedAt
);
