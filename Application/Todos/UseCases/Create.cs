using Domain.Interfaces;
using Application.Shared.UseCases;
using Domain.Todos.Entities;
using Application.Todos.Models;

namespace Application.Todos.UseCases;

public class Create
    : CreateBase<Todo, TodoResultDTO, TodoCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Todo> repository,
            IDomainService<Todo> domain
        ) : base(repository, domain)
        {
        }

        protected override Todo CreateDomain(Command request)
            => Todo.CreateNew(
                title: new(request.Item.Title),
                description: new(request.Item.Description),
                period: new(request.Item.StartDate, request.Item.EndDate),
                state: new(request.Item.State),
                ownerUserId: request.UserId
            );

        protected override TodoResultDTO CreateDTO(Todo item) => new(item);
    }
}
