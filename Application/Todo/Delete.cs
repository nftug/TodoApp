using Domain.Todo;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todo;

public class Delete : DeleteBase<TodoModel>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<TodoModel> repository,
            IDomainService<TodoModel> domain
        ) : base(repository, domain)
        {
        }
    }
}
