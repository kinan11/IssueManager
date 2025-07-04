using IssueManager.Core.Interfaces;
using IssueManager.Core.Models.AddIssue;
using IssueManager.Core.Models.CloseIssue;
using IssueManager.Core.Models.UpdateIssue;

namespace IssueManager.Core.Services
{
    public class GitLabIssueService : IIssueService
    {
        public async Task<AddIssueResponse> AddIssueAsync(AddIssueRequest addIssueRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<CloseIssueResponse> CloseIssueAsync(CloseIssueRequest updateIssueRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<UpdateIssueResponse> UpdateIssueAsync(UpdateIssueRequest updateIssueRequest)
        {
            throw new NotImplementedException();
        }
    }
}
