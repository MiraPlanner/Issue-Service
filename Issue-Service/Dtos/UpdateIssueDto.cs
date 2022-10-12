using System.ComponentModel.DataAnnotations;
using Issue_Service.Models;

namespace Issue_Service.Dtos;

public record UpdateIssueDto(
    [Required] string Title, 
    string? Description, 
    [Required] int Duration, 
    [Required] IssueType IssueType
);