using Domain.Todos;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todos;

public class Details : DetailsBase<Todo, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo> repository)
            : base(repository)
        {
        }

        protected override TodoResultDTO CreateDTO(Todo item) => new(item);
    }
}
