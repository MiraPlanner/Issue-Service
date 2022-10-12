using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Issue_Service.Models;

namespace Issue_Service.Dtos;

public record CreateIssueDto(
    [Required] string Title, 
    [Optional] string? Description, 
    [Range(0,1000000)] int Duration, 
    [Required] IssueType IssueType
);