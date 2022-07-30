using Domain.Interfaces;
using Application.Shared.UseCases;
using Domain.Todos.Entities;

namespace Application.Todos.UseCases;

public class Delete : DeleteBase<Todo>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Todo> repository,
            IDomainService<Todo> domain
        ) : base(repository, domain)
        {
        }
    }
}
