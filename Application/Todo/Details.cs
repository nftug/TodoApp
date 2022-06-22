using Domain.Todo;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todo;

public class Details : DetailsBase<TodoModel, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<TodoModel> repository)
            : base(repository)
        {
        }

        protected override TodoResultDTO CreateDTO(TodoModel item) => new(item);
    }
}
