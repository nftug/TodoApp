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
            IQueryService<TodoModel> querySearch,
            IDomainService<TodoModel> domain
        ) : base(repository, querySearch, domain)
        {
        }

        protected override TodoResultDTO CreateDTO(TodoModel item) => new(item);
    }
}
