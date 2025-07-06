namespace IssueManager.Core.Models
{
    /// <summary>
    /// Request to add a new issue to a repository.
    /// </summary>
    public class AddIssueRequest
    {
        /// <summary>
        /// A description of the issue.
        /// </summary>
        public required string Description { get; set; }
        /// <summary>
        /// The repository where the issue should be created.
        /// </summary>
        public required RepoDto Repo { get; set; }
        /// <summary>
        /// The title of the issue.
        /// </summary>
        public required string Title { get; set; }
    }
}
