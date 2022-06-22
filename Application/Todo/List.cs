using Domain.Todo;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todo;

public class List : ListBase<TodoModel, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<TodoModel> repository,
            IQuerySearch<TodoModel> querySearch
        )
            : base(repository, querySearch)
        {
        }

        protected override TodoResultDTO CreateDTO(TodoModel item) => new(item);
    }
}
