namespace IssueManager.Core.Models
{
    /// <summary>
    /// Request to update an existing issue in a repository.
    /// </summary>
    public class UpdateIssueRequest
    {
        /// <summary>
        /// The new description for the issue.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// The repository that contains the issue.
        /// </summary>
        public required RepoDto Repo { get; set; }
        /// <summary>
        /// The new title for the issue.
        /// </summary>
        public string? Title { get; set; }
    }
}
