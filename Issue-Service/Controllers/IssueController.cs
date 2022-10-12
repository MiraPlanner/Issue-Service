using Microsoft.AspNetCore.Mvc;
using Mira_Common;
using Issue_Service.Dtos;
using Issue_Service.Interfaces;
using Issue_Service.Models;

namespace Issue_Service.Controllers;

[ApiController]
[Route("issues")]
public class IssueController : ControllerBase
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly IIssueService _issueService;
    /*
    private readonly IPublishEndpoint publishEndpoint;
    */

    /*public IssueController(IRepository<Issue> issueRepository, IPublishEndpoint publishEndpoint)
    {
        this.issueRepository = issueRepository;
        this.publishEndpoint = publishEndpoint;
    }*/
    
    public IssueController(IRepository<Issue> issueRepository, IIssueService issueService)
    {
        _issueRepository = issueRepository;
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
        
        // This will be a contract from a common lib
        // Params will go inside 
        /*
        await publishEndpoint.Publish(new IssueCreated());
        */
        return CreatedAtAction(nameof(GetById), new { id = issue.Id }, issue);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateIssueDto updateIssueDto)
    {
        var issue = await _issueService.Update(id, updateIssueDto);

        if (issue == null) return NotFound();
        
        // This will be a contract from a common lib
        // Params will go inside 
        /*
        await publishEndpoint.Publish(new IssueUpdated(issue.Id, issue.Name, issue.Description));
        */

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var issue = await _issueService.Delete(id);

        if (issue == null) return NotFound();

        // This will be a contract from a common lib
        // Params will go inside 
        /*
        await publishEndpoint.Publish(new IssueDeleted());
        */

        return NoContent();
    }


}