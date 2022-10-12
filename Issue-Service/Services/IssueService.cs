using Mira_Common;
using Issue_Service.Dtos;
using Issue_Service.Interfaces;
using Issue_Service.Models;

namespace Issue_Service.Services;

public class IssueService : IIssueService
{
    private readonly IRepository<Issue> _issueRepository;

    public IssueService(IRepository<Issue> issueRepository)
    {
        _issueRepository = issueRepository;
    }
    
    public async Task<IEnumerable<IssueDto>> GetAll()
    {
        var issues = (await _issueRepository.GetAll()).Select(issue => issue.AsDto());

        return issues;
    }

    public async Task<IssueDto> GetById(Guid id)
    {
        var issue = await _issueRepository.Get(id);

        if (issue == null) return null!;
        
        return issue.AsDto();
    }

    public async Task<IssueDto> Create(CreateIssueDto createIssueDto)
    {
        var issue = new Issue
        {
            Title = createIssueDto.Title,
            Description = createIssueDto.Description,
            Duration = createIssueDto.Duration,
            IssueType = createIssueDto.IssueType,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };

        await _issueRepository.Create(issue);
        
        return issue.AsDto();
    }

    public async Task<IssueDto?> Update(Guid id, UpdateIssueDto updateIssueDto)
    {
        var issue = await _issueRepository.Get(id);

        if (issue == null) return null!;

        issue.Title = updateIssueDto.Title;
        issue.Description = updateIssueDto.Description;
        issue.Duration = updateIssueDto.Duration;
        issue.IssueType = updateIssueDto.IssueType;
        issue.UpdatedAt = DateTimeOffset.Now;
        
        await _issueRepository.Update(issue);

        return issue.AsDto();
    }

    public async Task<IssueDto?> Delete(Guid id)
    {
        var item = await _issueRepository.Get(id);

        if (item == null) return null;

        await _issueRepository.Delete(item.Id);
        
        return item.AsDto();
    }
}