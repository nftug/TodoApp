using Domain.Todos;
using Domain.Interfaces;
using Application.Shared.UseCase;

namespace Application.Todos;

public class Create
    : CreateBase<Todo, TodoResultDTO, TodoCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Todo> repository)
            : base(repository)
        {
        }

        protected override Todo CreateDomain(Command request)
            => Todo.CreateNew(
                title: new(request.Item.Title!),
                description: new(request.Item.Description),
                period: new(request.Item.BeginDateTime, request.Item.DueDateTime),
                state: new(request.Item.State),
                ownerUserId: request.UserId
            );

        protected override TodoResultDTO CreateDTO(Todo item) => new(item);
    }
}
