using Microsoft.AspNetCore.Mvc;
using Issue_Service.Dtos;
using Issue_Service.Interfaces;

namespace Issue_Service.Controllers;

[ApiController]
[Route("issues")]
public class IssueController : ControllerBase
{
    private readonly IIssueService _issueService;
   
    public IssueController(IIssueService issueService)
    {
        _issueService = issueService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IssueDto>>> GetAll()
    {
        var issues = await _issueService.GetAll();

        return Ok(issues);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IssueDto>> GetById(Guid id)
    {
        var issue = await _issueService.GetById(id);

        if (issue == null) return NotFound();

        return Ok(issue);
    }

    [HttpPost]
    public async Task<ActionResult<IssueDto>> Create(CreateIssueDto createIssueDto)
    {
        var issue = await _issueService.Create(createIssueDto);
        
        return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issue);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateIssueDto updateIssueDto)
    {
        var issue = await _issueService.Update(id, updateIssueDto);

        if (issue == null) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var issue = await _issueService.Delete(id);

        if (issue == null) return NotFound();

        return NoContent();
    }
}