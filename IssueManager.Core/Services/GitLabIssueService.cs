using IssueManager.Core.Interfaces;
using IssueManager.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IssueManager.Core.Services
{
    /// <summary>
    /// Issue service for GitLab repositories.
    /// </summary>
    public class GitLabIssueService : BaseIssueService, IIssueService
    {
        private readonly string _accessToken;
        private const string ApiBaseUrl = "https://gitlab.com/api/v4/projects/";

        public GitLabIssueService(IConfiguration config, HttpClient client)
            : base(client)
        {
            _accessToken = config.GetSection("AccessTokens")["GitLab"]
                ?? throw new InvalidOperationException("GitLab token is not configured.");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        public async Task<int> AddIssueAsync(AddIssueRequest request)
        {
            var project = Uri.EscapeDataString($"{request.Repo.Owner}/{request.Repo.Repo}");
            var url = $"{ApiBaseUrl}{project}/issues";
            var body = new { title = request.Title, description = request.Description };

            var response = await SendJsonRequestAsync(HttpMethod.Post, url, body);
            using var doc = JsonDocument.Parse(response);

            return doc.RootElement.GetProperty("iid").GetInt32();
        }

        public async Task CloseIssueAsync(int issueNumber, RepoDto repo)
        {
            var project = Uri.EscapeDataString($"{repo.Owner}/{repo.Repo}");
            var url = $"{ApiBaseUrl}{project}/issues/{issueNumber}";
            var body = new { state_event = "close" };

            await SendJsonRequestAsync(HttpMethod.Put, url, body);
        }

        public async Task UpdateIssueAsync(int issueNumber, UpdateIssueRequest request)
        {
            var project = Uri.EscapeDataString($"{request.Repo.Owner}/{request.Repo.Repo}");
            var url = $"{ApiBaseUrl}{project}/issues/{issueNumber}";
            var body = new { title = request.Title, description = request.Description };

            await SendJsonRequestAsync(HttpMethod.Put, url, body);
        }
    }
}
