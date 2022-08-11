using Application.Shared.UseCases;
using Domain.Todos.Entities;
using Application.Todos.Models;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Todos.UseCases;

public class Put : EditBase<Todo, TodoResultDTO, TodoCommand>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo> repository, IDomainService<Todo> domain)
            : base(repository, domain)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo command) => new(command);

        protected override void Edit(Todo origin, TodoCommand command)
        {
            origin.Edit(
                title: new(command.Title),
                description: new(command.Description),
                period: new(command.StartDate, command.EndDate),
                state: new(command.State)
            );
        }
    }
}
