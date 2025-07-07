using System.Net;
using System.Text;
using IssueManager.Core.Services;
using Microsoft.Extensions.Configuration;

namespace IssueManager.Tests.Services
{
    public class IssueServiceFactoryTests
    {
        private readonly IssueServiceFactory _factory;
        private readonly GitHubIssueService _gitHubService;
        private readonly GitLabIssueService _gitLabService;

        public IssueServiceFactoryTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "AccessTokens:GitHub", "fake-github-token" },
                { "AccessTokens:GitLab", "fake-gitlab-token" },
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var fakeHandler = new FakeHttpMessageHandler();
            var httpClient = new HttpClient(fakeHandler);

            _gitHubService = new GitHubIssueService(configuration, httpClient);
            _gitLabService = new GitLabIssueService(configuration, httpClient);
            _factory = new IssueServiceFactory(_gitHubService, _gitLabService);
        }

        [Theory]
        [InlineData("github")]
        [InlineData("GITHUB")]
        public void GetService_GitHubProvider_ReturnsGitHubService(string provider)
        {
            var service = _factory.GetService(provider);
            Assert.Same(_gitHubService, service);
        }

        [Theory]
        [InlineData("gitlab")]
        [InlineData("GITLAB")]
        public void GetService_GitLabProvider_ReturnsGitLabService(string provider)
        {
            var service = _factory.GetService(provider);
            Assert.Same(_gitLabService, service);
        }

        [Theory]
        [InlineData("bitbucket")]
        [InlineData("")]
        public void GetService_UnsupportedProvider_ThrowsException(string provider)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                _factory.GetService(provider)
            );
            Assert.Contains("Unsupported provider", ex.Message);
        }

        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken
            )
            {
                var json = """{ "number": 1 }""";
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                };

                return Task.FromResult(response);
            }
        }
    }
}
