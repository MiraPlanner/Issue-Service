using Issue_Service.Dtos;

namespace Issue_Service.Interfaces;

public interface IIssueService
{
    public Task<IEnumerable<IssueDto>> GetAll();
    public Task<IssueDto?> GetById(Guid id);
    public Task<IssueDto?> Create(CreateIssueDto createIssueDto);
    public Task<IssueDto?> Update(Guid id, UpdateIssueDto updateIssueDto);
    public Task<IssueDto?> Delete(Guid id);
}