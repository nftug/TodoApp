using Microsoft.AspNetCore.Mvc;
using Application.Todos;
using Persistence.Todos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ApiControllerBase
{
    // GET: api/TodoItems
    [HttpGet]
    public async Task<IActionResult> GetTodoItems([FromQuery] TodoQueryParameter param)
    {
        return await HandleResult(() => Mediator.Send(new List.Query(param, _userId)));
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoItem(Guid id)
    {
        return await HandleResult(() => Mediator.Send(new Details.Query(id, _userId)));
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(Guid id, TodoCommandDTO TodoCommandDTO)
    {
        return await HandleResult(() => Mediator.Send(new Edit.Command(id, TodoCommandDTO, _userId)));
    }

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<IActionResult> PostTodoItem(TodoCommandDTO TodoCommandDTO)
    {
        return await HandleResult(() => Mediator.Send(new Create.Command(TodoCommandDTO, _userId)));
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id)
    {
        return await HandleResult(() => Mediator.Send(new Delete.Command(id, _userId)));
    }
}
