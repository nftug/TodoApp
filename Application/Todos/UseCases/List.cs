using Domain.Interfaces;
using Application.Shared.UseCases;
using Domain.Todos.Entities;
using Application.Todos.Models;

namespace Application.Todos.UseCases;

public class List : ListBase<Todo, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Todo> repository,
            IDomainService<Todo> domain
        ) : base(repository, domain)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo item) => new(item);
    }
}
