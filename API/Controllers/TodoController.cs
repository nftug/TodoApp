using Microsoft.AspNetCore.Mvc;
using Application.Todo;
using Infrastructure.Todo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class TodoController : ApiControllerBase
{
    // GET: api/Todo
    [HttpGet]
    public async Task<IActionResult> GetTodo([FromQuery] TodoQueryParameter param)
    {
        return await HandleRequest(new List.Query(param, _userId));
    }

    // GET: api/Todo/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodo(Guid id)
    {
        return await HandleRequest(new Details.Query(id, _userId));
    }

    // PUT: api/Todo/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodo(Guid id, TodoCommandDTO TodoCommandDTO)
    {
        return await HandleRequest(new Edit.Command(id, TodoCommandDTO, _userId));
    }

    // POST: api/Todo
    [HttpPost]
    public async Task<IActionResult> PostTodo(TodoCommandDTO TodoCommandDTO)
    {
        return await HandleRequest(new Create.Command(TodoCommandDTO, _userId));
    }

    // DELETE: api/Todo/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        return await HandleRequest(new Delete.Command(id, _userId));
    }
}
