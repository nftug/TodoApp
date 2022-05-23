using Microsoft.AspNetCore.Mvc;
using Application.Todos;
using Infrastructure.Todos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class TodosController : ApiControllerBase
{
    // GET: api/Todos
    [HttpGet]
    public async Task<IActionResult> GetTodos([FromQuery] TodoQueryParameter param)
    {
        return await HandleResult(() => Mediator.Send(new List.Query(param, _userId)));
    }

    // GET: api/Todos/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodo(Guid id)
    {
        return await HandleResult(() => Mediator.Send(new Details.Query(id, _userId)));
    }

    // PUT: api/Todos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodo(Guid id, TodoCommandDTO TodoCommandDTO)
    {
        return await HandleResult(() => Mediator.Send(new Edit.Command(id, TodoCommandDTO, _userId)));
    }

    // POST: api/Todos
    [HttpPost]
    public async Task<IActionResult> PostTodo(TodoCommandDTO TodoCommandDTO)
    {
        return await HandleResult(() => Mediator.Send(new Create.Command(TodoCommandDTO, _userId)));
    }

    // DELETE: api/Todos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        return await HandleResult(() => Mediator.Send(new Delete.Command(id, _userId)));
    }
}
