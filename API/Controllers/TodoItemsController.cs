using Microsoft.AspNetCore.Mvc;
using Application.TodoItems;
using Application.TodoItems.Query;
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
    public async Task<IActionResult> GetTodoItems([FromQuery] QueryParameter param)
    {
        return HandleResult(await Mediator.Send(new List.Query(param, _userId)));
    }

    // GET: api/TodoItems/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodoItem(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query(id, _userId)));
    }

    // PUT: api/TodoItems/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(Guid id, TodoItemDTO todoItemDTO)
    {
        return HandleResult(await Mediator.Send(new Edit.Command(id, todoItemDTO, _userId)));
    }

    // POST: api/TodoItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<IActionResult> PostTodoItem(TodoItemDTO todoItemDTO)
    {
        return HandleResult(await Mediator.Send(new Create.Command(todoItemDTO, _userId)));
    }

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command(id, _userId)));
    }
}
