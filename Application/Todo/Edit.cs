using Domain.Todo;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todo;

public class Edit
    : EditBase<TodoModel, TodoResultDTO, TodoCommandDTO>
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

        protected override void Put(TodoModel item, Command request)
        {
            item.Edit(
                title: new(request.Item.Title!),
                description: new(request.Item.Description),
                period: new(request.Item.StartDate, request.Item.EndDate),
                state: new(request.Item.State)
            );
        }
    }
}
