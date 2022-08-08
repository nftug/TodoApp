using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Shared.Enums;
using Domain.Todos.Queries;
using Application.Todos.UseCases;
using Application.Todos.Models;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TodoController : ApiControllerBase
{
    // GET: api/Todo
    [HttpGet]
    public async Task<IActionResult> GetTodo([FromQuery] TodoQueryParameter param)
        => await HandleRequest(new List.Query(param, _userId));

    // GET: api/Todo/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodo(Guid id)
        => await HandleRequest(new Details.Query(id, _userId));

    // PUT: api/Todo/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodo(Guid id, TodoCommandDTO TodoCommandDTO)
        => await HandleRequest(new Edit.Command(id, TodoCommandDTO, _userId, EditMode.Put));

    // PATCH: api/Todo/5
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTodo(Guid id, TodoCommandDTO TodoCommandDTO)
        => await HandleRequest(new Edit.Command(id, TodoCommandDTO, _userId, EditMode.Patch));

    // POST: api/Todo
    [HttpPost]
    public async Task<IActionResult> PostTodo(TodoCommandDTO TodoCommandDTO)
        => await HandleRequest(new Create.Command(TodoCommandDTO, _userId));

    // DELETE: api/Todo/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
        => await HandleRequest(new Delete.Command(id, _userId));

    // PUT: api/Todo/5
    [HttpPut("{id}/state")]
    public async Task<IActionResult> PutTodoState(Guid id, TodoStateCommand command)
        => await HandleRequest(new EditState.Command(id, command, _userId, EditMode.Put));
}
