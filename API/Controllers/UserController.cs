using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Shared.Enums;
using Domain.Comments.Queries;
using Application.Users.UseCases;
using Application.Users.Models;
using Domain.Todos.Queries;

namespace API.Controllers.Account;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(Guid id)
        => await HandleRequest(new Details.Public.Query(id, _userId));

    [HttpGet("me")]
    public async Task<IActionResult> GetMyUserInfo()
        => await HandleRequest(new Details.Me.Query(_userId, _userId));

    [HttpPut("me")]
    public async Task<IActionResult> PutMyUserInfo(UserCommandDTO user)
        => await HandleRequest(new Edit.Command(_userId, user, _userId, EditMode.Put));

    [HttpPatch("me")]
    public async Task<IActionResult> PatchMyUserInfo(UserCommandDTO user)
        => await HandleRequest(new Edit.Command(_userId, user, _userId, EditMode.Patch));

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMyUser()
        => await HandleRequest(new Delete.Command(_userId, _userId));

    [HttpGet("me/todo")]
    public async Task<IActionResult> GetMyTodo([FromQuery] TodoQueryParameter param)
    {
        param.UserId = _userId;
        return await HandleRequest(new Application.Todos.UseCases.List.Query(param, _userId));
    }

    [HttpGet("me/comment")]
    public async Task<IActionResult> GetMyComment([FromQuery] CommentQueryParameter param)
    {
        param.UserId = _userId;
        return await HandleRequest(new Application.Comments.UseCases.List.Query(param, _userId));
    }
}
