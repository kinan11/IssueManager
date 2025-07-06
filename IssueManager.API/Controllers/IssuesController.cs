using IssueManager.Core.Interfaces;
using IssueManager.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace IssueManager.API.Controllers
{
    /// <summary>
    /// Controller that manages issues on supported Git hosting services.
    /// </summary>
    /// <param name="issueServiceFactory">Factory to resolve the appropriate <see cref="IIssueService"/>.</param>
    [ApiController]
    [Route("issues")]
    public class IssuesController(IIssueServiceFactory issueServiceFactory) : ControllerBase
    {
        /// <summary>
        /// Adds a new issue to the selected Git hosting service.
        /// </summary>
        /// <param name="addIssueRequest">Request containing issue details and provider.</param>
        /// <returns>The result of the add operation.</returns>
        [HttpPost("addIssue")]
        public async Task<ActionResult<int>> AddIssue([FromBody] AddIssueRequest addIssueRequest)
        {
            var service = issueServiceFactory.GetService(addIssueRequest.Repo.Provider);
            var result = await service.AddIssueAsync(addIssueRequest);
            return StatusCode(201, result);
        }

        /// <summary>
        /// Closes an existing issue on the selected Git hosting service.
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <param name="repo">Request containing issue identifier and provider.</param>
        /// <returns>The result of the close operation.</returns>
        [HttpPatch("closeIssue/{issueNumber}")]
        public async Task<ActionResult> CloseIssue(int issueNumber, [FromBody] RepoDto repo)
        {
            var service = issueServiceFactory.GetService(repo.Provider);
            await service.CloseIssueAsync(issueNumber, repo);
            return Ok();
        }

        /// <summary>
        /// Updates an existing issue on the selected Git hosting service.
        /// </summary>
        /// <param name="issueNumber"></param>
        /// <param name="updateIssueRequest">Request containing updated issue details and provider.</param>
        /// <returns>The result of the update operation.</returns>
        [HttpPatch("updateIssue/{issueNumber}")]
        public async Task<ActionResult> UpdateIssue(int issueNumber, [FromBody] UpdateIssueRequest updateIssueRequest)
        {
            var service = issueServiceFactory.GetService(updateIssueRequest.Repo.Provider);
            await service.UpdateIssueAsync(issueNumber, updateIssueRequest);
            return Ok();
        }
    }
}
