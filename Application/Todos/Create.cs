using MediatR;
using Domain.Todos;

namespace Application.Todos;

public class Create
{
    public class Command : IRequest<TodoResultDTO>
    {
        public TodoCommandDTO TodoCommandDTO { get; set; }
        public string UserId { get; set; }

        public Command(TodoCommandDTO todoItemDTO, string usedId)
        {
            TodoCommandDTO = todoItemDTO;
            UserId = usedId;
        }
    }

    public class Handler : IRequestHandler<Command, TodoResultDTO>
    {
        private readonly ITodoRepository _todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoResultDTO> Handle(Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.TodoCommandDTO;

            var todo = Todo.CreateNew(
                title: new TodoTitle(inputItem.Title!),
                description: !string.IsNullOrWhiteSpace(inputItem.Description) ?
                    new TodoDescription(inputItem.Description) : null,
                period: new TodoPeriod(inputItem.BeginDateTime, inputItem.DueDateTime),
                state: inputItem.State != null ?
                    new TodoState((int)inputItem.State) : TodoState.Todo,
                ownerUserId: request.UserId
            );

            var result = await _todoRepository.CreateAsync(todo);
            return TodoResultDTO.CreateResultDTO(result);
        }
    }
}
