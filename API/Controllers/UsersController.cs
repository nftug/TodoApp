using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Todos;
using Infrastructure.Comments;
using AutoMapper;

namespace API.Controllers.Account;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(string id)
    {
        return await HandleResult(() => Mediator.Send(new Details.Public.Query(id, _userId)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyUserInfo()
    {
        return await HandleResult(() => Mediator.Send(new Details.Me.Query(_userId)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("me")]
    public async Task<IActionResult> PutMyUserInfo(UserCommandDTO user)
    {
        return await HandleResult(() => Mediator.Send(new Edit.Command(user, _userId)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me/todos")]
    public async Task<IActionResult> GetMyTodos([FromQuery] TodoQueryParameter param)
    {
        param.UserId = _userId;
        return await HandleResult(() => Mediator.Send(new Application.Todos.List.Query(param, _userId)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me/comments")]
    public async Task<IActionResult> GetMyComments([FromQuery] CommentQueryParameter param)
    {
        param.UserId = _userId;
        return await HandleResult(() => Mediator.Send(new Application.Comments.List.Query(param, _userId)));
    }
}
