using Domain.Todos;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todos;

public class List : ListBase<Todo, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Todo> repository,
            IQuerySearch<Todo> querySearch
        )
            : base(repository, querySearch)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo item) => new(item);
    }
}
