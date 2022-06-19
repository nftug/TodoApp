using Domain.Todos;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Application.Shared.UseCase;

namespace Application.Todos;

public class List : ListBase<Todo, TodoDataModel, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Todo, TodoDataModel> repository,
            IQuerySearch<TodoDataModel> querySearch
        )
            : base(repository, querySearch)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo item) => new(item);
    }
}
