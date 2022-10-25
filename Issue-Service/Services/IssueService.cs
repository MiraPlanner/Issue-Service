using Mira_Common;
using Issue_Service.Dtos;
using Issue_Service.Interfaces;
using Issue_Service.Models;
using MassTransit;
using Mira_Contracts.IssueContracts;

namespace Issue_Service.Services;

public class IssueService : IIssueService
{
    private readonly IRepository<Issue> _issueRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public IssueService(IRepository<Issue> issueRepository, IPublishEndpoint publishEndpoint)
    {
        _issueRepository = issueRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<IEnumerable<IssueDto>> GetAll()
    {
        var issues = (await _issueRepository.GetAll()).Select(issue => issue.AsDto());

        return issues;
    }

    public async Task<IssueDto?> GetById(Guid id)
    {
        var issue = await _issueRepository.Get(id);

        if (issue == null) return null;
        
        return issue.AsDto();
    }

    public async Task<IssueDto> Create(CreateIssueDto createIssueDto)
    {
        var issue = new Issue
        {
            SprintId = createIssueDto.SprintId,
            Title = createIssueDto.Title,
            Description = createIssueDto.Description,
            Duration = createIssueDto.Duration,
            IssueStatus = createIssueDto.IssueStatus,
            IssueType = createIssueDto.IssueType,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };
        await _issueRepository.Create(issue);

        var issueCreatedContract = new IssueCreated(
            issue.Id,
            issue.SprintId,
            issue.Title,
            issue.IssueStatus,
            issue.IssueType
        );
        await _publishEndpoint.Publish(issueCreatedContract);

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
        
        var issueUpdatedContract = new IssueUpdated(
            issue.Id,
            issue.SprintId,
            issue.Title,
            issue.IssueStatus,
            issue.IssueType
        );
        await _publishEndpoint.Publish(issueUpdatedContract);
        
        return issue.AsDto();
    }

    public async Task<IssueDto?> Delete(Guid id)
    {
        var item = await _issueRepository.Get(id);

        if (item == null) return null;

        await _issueRepository.Delete(item.Id);
        
        await _publishEndpoint.Publish(new IssueDeleted(item.Id));

        return item.AsDto();
    }
}