using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Users;
using Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers.Account;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ApiControllerBase
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserInfo(string id)
    {
        return HandleResult(await Mediator.Send(new Details.Public.Query(id, _userId)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyUserInfo()
    {
        return HandleResult(await Mediator.Send(new Details.Me.Query(_userId)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("me")]
    public async Task<IActionResult> EditMyUserInfo(UserDTO.Me user)
    {
        return HandleResult(await Mediator.Send(new Edit.Command(user, _userId)));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("me/todoItems")]
    public async Task<IActionResult>
        GetMyTodoItems([FromQuery] Application.TodoItems.Query.QueryParameter param)
    {
        param.User = _userId;
        return HandleResult(
            await Mediator.Send(new Application.TodoItems.List.Query(param, _userId))
        );
    }
}
