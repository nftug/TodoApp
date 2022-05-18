using Microsoft.AspNetCore.Mvc;
using Application.Comments;
using Infrastructure.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class CommentsController : ApiControllerBase
{
    // GET: api/Comments
    [HttpGet]
    public async Task<IActionResult> GetComments([FromQuery] CommentQueryParameter param)
    {
        return await HandleResult(() => Mediator.Send(new List.Query(param, _userId)));
    }

    // GET: api/Comments/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(Guid id)
    {
        return await HandleResult(() => Mediator.Send(new Details.Query(id, _userId)));
    }

    // PUT: api/Comments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(Guid id, CommentCommandDTO commentCommandDTO)
    {
        return await HandleResult(() => Mediator.Send(new Edit.Command(id, commentCommandDTO, _userId, false)));
    }

    // PATCH: api/Comments/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PatchComment(Guid id, CommentCommandDTO commentCommandDTO)
    {
        return await HandleResult(() => Mediator.Send(new Edit.Command(id, commentCommandDTO, _userId, true)));
    }

    // POST: api/Comments
    [HttpPost]
    public async Task<IActionResult> PostComment(CommentCommandDTO commentCommandDTO)
    {
        return await HandleResult(() => Mediator.Send(new Create.Command(commentCommandDTO, _userId)));
    }

    // DELETE: api/Comments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        return await HandleResult(() => Mediator.Send(new Delete.Command(id, _userId)));
    }
}
