using Application.Shared.UseCases;
using Domain.Todos.Entities;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Todos.DTOs;

namespace Application.Todos.UseCases;

public class List : ListBase<Todo, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IFilterQueryService<Todo> repository, IDomainService<Todo> domain)
            : base(repository, domain)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo item) => new(item);
    }
}
