using IssueManager.Core.Interfaces;

namespace IssueManager.Core.Services
{
    /// <summary>
    /// Factory for providing the appropriate implementation of <see cref="IIssueService"/>
    /// based on the selected Git hosting provider.
    /// </summary>
    /// <param name="gitHubService">An instance of <see cref="GitHubIssueService"/>.</param>
    /// <param name="gitLabService">An instance of <see cref="GitLabIssueService"/>.</param>
    public class IssueServiceFactory(
        GitHubIssueService gitHubService,
        GitLabIssueService gitLabService
    ) : IIssueServiceFactory
    {
        /// <summary>
        /// Returns the appropriate <see cref="IIssueService"/> implementation
        /// for the specified Git hosting provider.
        /// </summary>
        /// <param name="provider">The Git hosting provider.</param>
        /// <returns>An implementation of <see cref="IIssueService"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unsupported provider is specified.</exception>
        public IIssueService GetService(string provider)
        {
            return provider.ToLowerInvariant() switch
            {
                "github" => gitHubService,
                "gitlab" => gitLabService,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(provider),
                    $"Unsupported provider: {provider}"
                ),
            };
        }
    }
}
