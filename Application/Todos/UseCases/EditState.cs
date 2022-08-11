using Application.Shared.UseCases;
using Application.Todos.Models;
using Domain.Services;
using Domain.Shared.Interfaces;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;

namespace Application.Todos.UseCases;

public class EditState : EditBase<Todo, TodoResultDTO, TodoStateCommand>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo> repository, IDomainService<Todo> domain)
            : base(repository, domain)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo command) => new(command);

        protected override void Put(Todo origin, TodoStateCommand command)
        {
            Patch(origin, command);
        }

        protected override void Patch(Todo origin, TodoStateCommand command)
        {
            origin.SetState(new(command.State));
        }
    }
}
