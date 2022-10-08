using Application.Shared.UseCases;
using Domain.Services;
using Domain.Shared.Interfaces;
using Domain.Todos.DTOs;
using Domain.Todos.Entities;

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

        protected override void Edit(Todo origin, TodoStateCommand command)
        {
            origin.SetState(new(command.State));
        }
    }
}
