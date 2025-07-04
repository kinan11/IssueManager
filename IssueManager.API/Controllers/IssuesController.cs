using IssueManager.Core.Models.AddIssue;
using IssueManager.Core.Models.CloseIssue;
using IssueManager.Core.Models.UpdateIssue;
using Microsoft.AspNetCore.Mvc;

namespace IssueManager.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class IssuesController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<AddIssueResponse>> AddIssue([FromBody] AddIssueRequest addIssueRequest)
        {
            return new AddIssueResponse();
        }

        [HttpPut]
        public async Task<ActionResult<CloseIssueResponse>> CloseIssue([FromBody] CloseIssueRequest closeIssueRequest)
        {
            return new CloseIssueResponse();
        }

        [HttpPatch]
        public async Task<ActionResult<UpdateIssueResponse>> UpdateIssue([FromBody] UpdateIssueRequest updateIssueRequest)
        {
            return new UpdateIssueResponse();
        }
    }
}
