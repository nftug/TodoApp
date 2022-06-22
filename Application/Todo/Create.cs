using Domain.Todo;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todo;

public class Create
    : CreateBase<TodoModel, TodoResultDTO, TodoCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<TodoModel> repository)
            : base(repository)
        {
        }

        protected override TodoModel CreateDomain(Command request)
            => TodoModel.CreateNew(
                title: new(request.Item.Title!),
                description: new(request.Item.Description),
                period: new(request.Item.StartDate, request.Item.EndDate),
                state: new(request.Item.State),
                ownerUserId: request.UserId
            );

        protected override TodoResultDTO CreateDTO(TodoModel item) => new(item);
    }
}
