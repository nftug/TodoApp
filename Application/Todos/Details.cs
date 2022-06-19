using Domain.Todos;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Application.Shared.UseCase;

namespace Application.Todos;

public class Details : DetailsBase<Todo, TodoDataModel, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo, TodoDataModel> repository)
            : base(repository)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo item) => new(item);
    }
}
