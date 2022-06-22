using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Todo;
using Infrastructure.Comment;

namespace API.Controllers.Account;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(Guid id)
    {
        return await HandleRequest(new DetailsPublic.Query(id, _userId));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyUserInfo()
    {
        return await HandleRequest(new DetailsMe.Query(_userId, _userId));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("me")]
    public async Task<IActionResult> PutMyUserInfo(UserCommandDTO user)
    {
        return await HandleRequest(new Edit.Command(_userId, user, _userId));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMyUser()
    {
        return await HandleRequest(new Delete.Command(_userId, _userId));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me/todos")]
    public async Task<IActionResult> GetMyTodo([FromQuery] TodoQueryParameter param)
    {
        param.UserId = _userId;
        return await HandleRequest(new Application.Todo.List.Query(param, _userId));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me/comments")]
    public async Task<IActionResult> GetMyComment([FromQuery] CommentQueryParameter param)
    {
        param.UserId = _userId;
        return await HandleRequest(new Application.Comment.List.Query(param, _userId));
    }
}
