using Application.Shared.UseCases;
using Domain.Todos.Entities;
using Domain.Todos.ValueObjects;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Todos.DTOs;

namespace Application.Todos.UseCases;

public class Patch : EditBase<Todo, TodoResultDTO, TodoPatchCommand>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo> repository, IDomainService<Todo> domain)
            : base(repository, domain)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo command) => new(command);

        protected override void Edit(Todo origin, TodoPatchCommand command)
        {
            TodoPeriod? period =
                command.StartDate != null && command.EndDate != null
                ? new TodoPeriod(command.StartDate, command.EndDate)
                : command.StartDate != null
                ? new TodoPeriod(command.StartDate, origin.Period.EndDateValue)
                : command.EndDate != null
                ? new TodoPeriod(origin.Period.StartDateValue, command.EndDate)
                : origin.Period;

            origin.Edit(
                title: command.Title != null ? new(command.Title) : origin.Title,
                description: command.Description != null ? new(command.Description) : origin.Description,
                period: period,
                state: command.State != null ? new(command.State) : origin.State
            );
        }
    }
}
