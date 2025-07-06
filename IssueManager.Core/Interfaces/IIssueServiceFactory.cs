namespace IssueManager.Core.Interfaces
{
    /// <summary>
    /// Provides a factory for resolving the correct <see cref="IIssueService"/>
    /// implementation based on the selected Git hosting service.
    /// </summary>
    public interface IIssueServiceFactory
    {
        /// <summary>
        /// Gets an instance of <see cref="IIssueService"/> for the specified provider.
        /// </summary>
        /// <param name="provider">The Git hosting service provider.</param>
        /// <returns>An implementation of <see cref="IIssueService"/> for the provider.</returns>
        public IIssueService GetService(string provider);
    }
}
