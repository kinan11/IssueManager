namespace IssueManager.Core.Models
{
    /// <summary>
    /// DTO containing repository information
    /// </summary>
    public class RepoDto
    {
        /// <summary>
        /// Repository owner
        /// </summary>
        public required string Owner { get; set; }
        /// <summary>
        /// Hosting Service
        /// </summary>
        public required string Provider { get; set; }
        /// <summary>
        /// Repository name
        /// </summary>
        public required string Repo { get; set; }
    }
}
