using Application.Shared.UseCases;
using Domain.Todos.Entities;
using Application.Todos.Models;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Todos.UseCases;

public class Details : DetailsBase<Todo, TodoResultDTO>
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
