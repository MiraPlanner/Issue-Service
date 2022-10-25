using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Sprint_Service.Models;

namespace Issue_Service.Dtos;

public record UpdateIssueDto(
    [Optional] Guid? SprintId,
    [Required] string Title, 
    [Optional] string? Description, 
    [Optional, Range(0,1000000)] int? Duration, 
    [Required] IssueStatus IssueStatus,
    [Required] IssueType IssueType
);