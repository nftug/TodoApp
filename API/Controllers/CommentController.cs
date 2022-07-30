using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Shared.Enums;
using Domain.Comments.Queries;
using Application.Comments.UseCases;
using Application.Comments.Models;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CommentController : ApiControllerBase
{
    // GET: api/Comment
    [HttpGet]
    public async Task<IActionResult> GetComment([FromQuery] CommentQueryParameter param)
        => await HandleRequest(new List.Query(param, _userId));

    // GET: api/Comment/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(Guid id)
        => await HandleRequest(new Details.Query(id, _userId));

    // PUT: api/Comment/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(Guid id, CommentCommandDTO commentCommandDTO)
        => await HandleRequest(new Edit.Command(id, commentCommandDTO, _userId, EditMode.Put));

    // PATCH: api/Comment/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchComment(Guid id, CommentCommandDTO commentCommandDTO)
        => await HandleRequest(new Edit.Command(id, commentCommandDTO, _userId, EditMode.Patch));

    // POST: api/Comment
    [HttpPost]
    public async Task<IActionResult> PostComment(CommentCommandDTO commentCommandDTO)
        => await HandleRequest(new Create.Command(commentCommandDTO, _userId));

    // DELETE: api/Comment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
        => await HandleRequest(new Delete.Command(id, _userId));
}
