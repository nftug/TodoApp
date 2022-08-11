using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Todos.Queries;
using Application.Todos.UseCases;
using Application.Todos.Models;
using Domain.Todos.Entities;
using MediatR;

namespace API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class TodoController
    : DomainControllerBase<Todo, TodoResultDTO, TodoCommand, TodoPatchCommand, TodoQueryParameter>
{
    public TodoController(ISender mediator) : base(mediator)
    {
    }

    [HttpPut("{id}/state")]
    public async Task<IActionResult> PutTodoState(Guid id, TodoStateCommand command)
        => await HandleRequest(new EditState.Command(id, command, UserId));
}
