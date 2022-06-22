using Microsoft.AspNetCore.Mvc;
using Application.Comment;
using Infrastructure.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class CommentController : ApiControllerBase
{
    // GET: api/Comment
    [HttpGet]
    public async Task<IActionResult> GetComment([FromQuery] CommentQueryParameter param)
    {
        return await HandleRequest(new List.Query(param, _userId));
    }

    // GET: api/Comment/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(Guid id)
    {
        return await HandleRequest(new Details.Query(id, _userId));
    }

    // PUT: api/Comment/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutComment(Guid id, CommentCommandDTO commentCommandDTO)
    {
        return await HandleRequest(new Edit.Command(id, commentCommandDTO, _userId));
    }

    // POST: api/Comment
    [HttpPost]
    public async Task<IActionResult> PostComment(CommentCommandDTO commentCommandDTO)
    {
        return await HandleRequest(new Create.Command(commentCommandDTO, _userId));
    }

    // DELETE: api/Comment/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        return await HandleRequest(new Delete.Command(id, _userId));
    }
}
