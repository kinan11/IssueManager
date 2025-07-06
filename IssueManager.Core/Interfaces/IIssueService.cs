using IssueManager.Core.Models;

namespace IssueManager.Core.Interfaces
{
    /// <summary>
    /// Defines operations for managing Git issues.
    /// </summary>
    public interface IIssueService
    {
        /// <summary>
        /// Adds a new issue to the repository.
        /// </summary>
        /// <param name="addIssueRequest">Request containing issue details and repository information.</param>
        /// <returns>The identifier of the created issue.</returns>
        Task<int> AddIssueAsync(AddIssueRequest addIssueRequest);

        /// <summary>
        /// Closes an existing issue.
        /// </summary>
        /// <param name="issueNumber">Issue identifier.</param>
        /// <param name="repo">Repository details.</param>
        Task CloseIssueAsync(int issueNumber, RepoDto repo);

        /// <summary>
        /// Updates an existing issue.
        /// </summary>
        /// <param name="issueNumber">Issue identifier.</param>
        /// <param name="updateIssueRequest">Request containing update information.</param>
        Task UpdateIssueAsync(int issueNumber, UpdateIssueRequest updateIssueRequest);
    }
}
