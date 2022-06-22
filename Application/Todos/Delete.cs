using Domain.Todos;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todos;

public class Delete : DeleteBase<Todo>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo> repository)
            : base(repository)
        {
        }
    }
}
