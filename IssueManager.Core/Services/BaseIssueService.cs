using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IssueManager.Core.Services
{
    /// <summary>
    /// Base class providing common HTTP functionality for issue services.
    /// </summary>
    public abstract class BaseIssueService
    {
        /// <summary>
        /// The shared <see cref="HttpClient"/> instance used for sending requests.
        /// </summary>
        protected readonly HttpClient Client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseIssueService"/> class.
        /// </summary>
        /// <param name="client">The HTTP client used for sending requests.</param>
        protected BaseIssueService(HttpClient client)
        {
            Client = client;
            Client.DefaultRequestHeaders.UserAgent.ParseAdd("IssueManagerApp/1.0");
        }

        /// <summary>
        /// Sends an HTTP request with JSON body and optional headers.
        /// </summary>
        /// <param name="method">HTTP method to use (e.g., POST, PUT, PATCH).</param>
        /// <param name="url">Target URL for the request.</param>
        /// <param name="body">The request body object to be serialized as JSON.</param>
        /// <param name="additionalHeaders">Optional additional headers to include.</param>
        /// <returns>The response body as a string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the response is not successful.</exception>

        protected async Task<string> SendJsonRequestAsync(HttpMethod method, string url, object body, Dictionary<string, string>? additionalHeaders = null)
        {
            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            using var response = await Client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"API error: {response.StatusCode} - {responseContent}");
            }

            return responseContent;
        }
    }
}
