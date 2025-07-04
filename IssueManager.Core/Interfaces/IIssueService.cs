using IssueManager.Core.Models.AddIssue;
using IssueManager.Core.Models.CloseIssue;
using IssueManager.Core.Models.UpdateIssue;

namespace IssueManager.Core.Interfaces
{
    public interface IIssueService
    {
        public Task<AddIssueResponse> AddIssueAsync(AddIssueRequest addIssueRequest);
        public Task<CloseIssueResponse> CloseIssueAsync(CloseIssueRequest updateIssueRequest);
        public Task<UpdateIssueResponse> UpdateIssueAsync(UpdateIssueRequest updateIssueRequest);
    }
}
