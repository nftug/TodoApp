using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Comments.Queries;
using Application.Users.UseCases;
using Domain.Todos.Queries;
using MediatR;
using Domain.Users.DTOs;

namespace API.Controllers.Account;

[Authorize]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    public UserController(ISender mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(Guid id)
        => await HandleRequest(new Details.Public.Query(id, UserId));

    [HttpGet("me")]
    public async Task<IActionResult> GetMyUserInfo()
        => await HandleRequest(new Details.Me.Query(UserId, UserId));

    [HttpPut("me")]
    public async Task<IActionResult> PutMyUserInfo(UserCommand user)
        => await HandleRequest(new Put.Command(UserId, user, UserId));

    [HttpPatch("me")]
    public async Task<IActionResult> PatchMyUserInfo(UserPatchCommand user)
        => await HandleRequest(new Patch.Command(UserId, user, UserId));

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMyUser()
        => await HandleRequest(new Delete.Command(UserId, UserId));

    [HttpGet("me/todo")]
    public async Task<IActionResult> GetMyTodo([FromQuery] TodoQueryParameter param)
    {
        param.UserId = UserId;
        return await HandleRequest(new Application.Todos.UseCases.List.Query(param, UserId));
    }

    [HttpGet("me/comment")]
    public async Task<IActionResult> GetMyComment([FromQuery] CommentQueryParameter param)
    {
        param.UserId = UserId;
        return await HandleRequest(new Application.Comments.UseCases.List.Query(param, UserId));
    }
}
