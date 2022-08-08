using Application.Shared.UseCases;
using Domain.Todos.Entities;
using Application.Todos.Models;
using Domain.Todos.ValueObjects;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Todos.UseCases;

public class Edit
    : EditBase<Todo, TodoResultDTO, TodoCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo> repository, IDomainService<Todo> domain)
            : base(repository, domain)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo command) => new(command);

        protected override void Put(Todo origin, TodoCommandDTO command)
        {
            origin.Edit(
                title: new(command.Title),
                description: new(command.Description),
                period: new(command.StartDate, command.EndDate),
                state: new(command.State)
            );
        }

        protected override void Patch(Todo origin, TodoCommandDTO command)
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
