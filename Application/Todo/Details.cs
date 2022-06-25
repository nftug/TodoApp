using Domain.Todo;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todo;

public class Details : DetailsBase<TodoModel, TodoResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<TodoModel> repository,
            IDomainService<TodoModel> domain
        ) : base(repository, domain)
        {
        }

        protected override TodoResultDTO CreateDTO(TodoModel item) => new(item);
    }
}
