using IssueManager.Core.Interfaces;
using IssueManager.Core.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IssueManager.Core.Services
{
    /// <summary>
    /// Issue service for GitHub repositories.
    /// </summary>
    public class GitHubIssueService : BaseIssueService, IIssueService
    {
        private readonly string _accessToken;
        private const string ApiBaseUrl = "https://api.github.com/repos/";

        public GitHubIssueService(IConfiguration config, HttpClient client)
            : base(client)
        {
            _accessToken = config.GetSection("AccessTokens")["GitHub"]
                ?? throw new InvalidOperationException("GitHub token is not configured.");
        }

        public async Task<int> AddIssueAsync(AddIssueRequest request)
        {
            var url = $"{ApiBaseUrl}{request.Repo.Owner}/{request.Repo.Repo}/issues";
            var body = new { title = request.Title, body = request.Description };

            var headers = GetAuthHeaders();
            headers["X-GitHub-Api-Version"] = "2022-11-28";
            headers["Authorization"] = $"Bearer {_accessToken}";

            var response = await SendJsonRequestAsync(HttpMethod.Post, url, body, headers);
            using var doc = JsonDocument.Parse(response);

            return doc.RootElement.GetProperty("number").GetInt32();
        }

        public async Task CloseIssueAsync(int issueNumber, RepoDto repo)
        {
            var url = $"{ApiBaseUrl}{repo.Owner}/{repo.Repo}/issues/{issueNumber}";
            var body = new { state = "closed" };

            await SendJsonRequestAsync(HttpMethod.Patch, url, body, GetAuthHeaders());
        }

        public async Task UpdateIssueAsync(int issueNumber, UpdateIssueRequest request)
        {
            var url = $"{ApiBaseUrl}{request.Repo.Owner}/{request.Repo.Repo}/issues/{issueNumber}";

            var body = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(request.Title)) body["title"] = request.Title;
            if (!string.IsNullOrWhiteSpace(request.Description)) body["body"] = request.Description;

            await SendJsonRequestAsync(HttpMethod.Patch, url, body, GetAuthHeaders());
        }

        private Dictionary<string, string> GetAuthHeaders() => new()
        {
            { "Authorization", $"Bearer {_accessToken}" },
            { "X-GitHub-Api-Version", "2022-11-28" }
        };
    }
}
