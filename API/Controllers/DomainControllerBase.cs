using Microsoft.AspNetCore.Mvc;
using Application.Shared.UseCases;
using Domain.Shared.Entities;
using Application.Shared.Interfaces;
using Domain.Shared.Queries;

namespace API.Controllers;

public abstract class DomainControllerBase<TDomain, TResultDTO, TCommand, TPatchCommand, TQueryParameter> : ApiControllerBase
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
    where TCommand : ICommand<TDomain>
    where TPatchCommand : ICommand<TDomain>
    where TQueryParameter : IQueryParameter<TDomain>
{
    protected abstract Commands<TDomain, TResultDTO, TCommand, TPatchCommand> Commands { get; }
    protected abstract Queries<TDomain, TResultDTO> Queries { get; }

    [HttpGet]
    public virtual async Task<IActionResult> GetList([FromQuery] TQueryParameter param)
        => await HandleRequest(Queries.List(param, UserId));

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetDetails(Guid id)
        => await HandleRequest(Queries.Details(id, UserId));

    [HttpPost]
    public virtual async Task<IActionResult> Post(TCommand command)
        => await HandleRequest(Commands.Create(command, UserId));

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Put(Guid id, TCommand command)
        => await HandleRequest(Commands.Put(id, command, UserId));

    [HttpPatch("{id}")]
    public virtual async Task<IActionResult> Patch(Guid id, TPatchCommand command)
            => await HandleRequest(Commands.Patch(id, command, UserId));

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(Guid id)
        => await HandleRequest(Commands.Delete(id, UserId));
}
