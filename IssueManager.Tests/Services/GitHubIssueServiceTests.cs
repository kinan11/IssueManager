using System.Net;
using IssueManager.Core.Models;
using IssueManager.Core.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace IssueManager.Tests.Services
{
    public class GitHubIssueServiceTests
    {
        private readonly HttpClient _client;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly GitHubIssueService _service;

        public GitHubIssueServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _client = new HttpClient(_handlerMock.Object);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string> { ["AccessTokens:GitHub"] = "test-token" }
                )
                .Build();

            _service = new GitHubIssueService(config, _client);
        }

        [Fact]
        public async Task AddIssueAsync_ReturnsIssueNumber()
        {
            var responseContent = "{\"number\": 123}";
            SetupHttpResponse(HttpMethod.Post, HttpStatusCode.Created, responseContent);

            var issueId = await _service.AddIssueAsync(
                new AddIssueRequest
                {
                    Title = "Test",
                    Description = "Description",
                    Repo = new RepoDto
                    {
                        Owner = "user",
                        Repo = "repo",
                        Provider = "gitub",
                    },
                }
            );

            Assert.Equal(123, issueId);
        }

        [Fact]
        public async Task CloseIssueAsync_Success()
        {
            SetupHttpResponse(HttpMethod.Patch, HttpStatusCode.OK);

            await _service.CloseIssueAsync(
                123,
                new RepoDto
                {
                    Owner = "user",
                    Repo = "repo",
                    Provider = "gitub",
                }
            );
        }

        [Fact]
        public async Task UpdateIssueAsync_WithValidFields_Success()
        {
            SetupHttpResponse(HttpMethod.Patch, HttpStatusCode.OK);

            await _service.UpdateIssueAsync(
                123,
                new UpdateIssueRequest
                {
                    Title = "Updated",
                    Description = "Updated description",
                    Repo = new RepoDto
                    {
                        Owner = "user",
                        Repo = "repo",
                        Provider = "gitub",
                    },
                }
            );
        }

        [Fact]
        public void Constructor_WithoutGitHubToken_ThrowsException()
        {
            var config = new ConfigurationBuilder().Build();
            var client = new HttpClient();

            var ex = Assert.Throws<InvalidOperationException>(() =>
                new GitHubIssueService(config, client)
            );
            Assert.Contains("GitHub token is not configured", ex.Message);
        }

        [Fact]
        public async Task UpdateIssueAsync_WithNoFields_DoesNotFail()
        {
            SetupHttpResponse(HttpMethod.Patch, HttpStatusCode.OK);

            var request = new UpdateIssueRequest
            {
                Title = "",
                Description = null,
                Repo = new RepoDto
                {
                    Owner = "user",
                    Repo = "repo",
                    Provider = "gitub",
                },
            };

            await _service.UpdateIssueAsync(123, request);
        }

        [Fact]
        public async Task AddIssueAsync_ResponseMissingNumber_ThrowsException()
        {
            var badJson = "{\"id\": 999}"; // brak `number`
            SetupHttpResponse(HttpMethod.Post, HttpStatusCode.Created, badJson);

            var request = new AddIssueRequest
            {
                Title = "test",
                Description = "desc",
                Repo = new RepoDto
                {
                    Owner = "user",
                    Repo = "repo",
                    Provider = "gitub",
                },
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AddIssueAsync(request));
        }

        private void SetupHttpResponse(
            HttpMethod method,
            HttpStatusCode status,
            string? content = ""
        )
        {
            _handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == method),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(
                    new HttpResponseMessage
                    {
                        StatusCode = status,
                        Content = new StringContent(content ?? ""),
                    }
                );
        }
    }
}
