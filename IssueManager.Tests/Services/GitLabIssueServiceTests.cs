using System.Net;
using IssueManager.Core.Models;
using IssueManager.Core.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;

namespace IssueManager.Tests.Services
{
    public class GitLabIssueServiceTests
    {
        private readonly HttpClient _client;
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly GitLabIssueService _service;

        public GitLabIssueServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _client = new HttpClient(_handlerMock.Object);

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string> { ["AccessTokens:GitLab"] = "test-token" }
                )
                .Build();

            _service = new GitLabIssueService(config, _client);
        }

        [Fact]
        public async Task AddIssueAsync_ApiError_ThrowsInvalidOperationException()
        {
            SetupHttpResponse(
                HttpMethod.Post,
                HttpStatusCode.BadRequest,
                "{\"message\":\"bad request\"}"
            );

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.AddIssueAsync(
                    new AddIssueRequest
                    {
                        Title = "Test",
                        Description = "Desc",
                        Repo = new RepoDto
                        {
                            Owner = "group",
                            Repo = "project",
                            Provider = "gitlab",
                        },
                    }
                )
            );

            Assert.Contains("API error", ex.Message);
            Assert.Contains("BadRequest", ex.Message);
        }

        [Fact]
        public async Task AddIssueAsync_ReturnsIssueIid()
        {
            var responseContent = "{\"iid\": 456}";
            SetupHttpResponse(HttpMethod.Post, HttpStatusCode.Created, responseContent);

            var result = await _service.AddIssueAsync(
                new AddIssueRequest
                {
                    Title = "Test",
                    Description = "Description",
                    Repo = new RepoDto
                    {
                        Owner = "group",
                        Repo = "project",
                        Provider = "gitlab",
                    },
                }
            );

            Assert.Equal(456, result);
        }

        [Fact]
        public async Task CloseIssueAsync_Success()
        {
            SetupHttpResponse(HttpMethod.Put, HttpStatusCode.OK);

            await _service.CloseIssueAsync(
                123,
                new RepoDto
                {
                    Owner = "group",
                    Repo = "project",
                    Provider = "gitlab",
                }
            );
        }

        [Fact]
        public void Constructor_WithoutToken_ThrowsInvalidOperationException()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection().Build(); // brak tokena

            var ex = Assert.Throws<InvalidOperationException>(() =>
                new GitLabIssueService(config, new HttpClient())
            );

            Assert.Equal("GitLab token is not configured.", ex.Message);
        }

        [Fact]
        public async Task UpdateIssueAsync_Success()
        {
            SetupHttpResponse(HttpMethod.Put, HttpStatusCode.OK);

            await _service.UpdateIssueAsync(
                123,
                new UpdateIssueRequest
                {
                    Title = "Updated",
                    Description = "Desc",
                    Repo = new RepoDto
                    {
                        Owner = "group",
                        Repo = "project",
                        Provider = "gitlab",
                    },
                }
            );
        }

        [Fact]
        public async Task UpdateIssueAsync_WithNullFields_Success()
        {
            SetupHttpResponse(HttpMethod.Put, HttpStatusCode.OK);

            var request = new UpdateIssueRequest
            {
                Title = null,
                Description = null,
                Repo = new RepoDto
                {
                    Owner = "group",
                    Repo = "project",
                    Provider = "gitlab",
                },
            };

            await _service.UpdateIssueAsync(123, request);
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
