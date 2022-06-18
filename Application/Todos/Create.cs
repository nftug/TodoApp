using MediatR;
using Domain.Todos;
using Infrastructure.DataModels;
using Domain.Interfaces;

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
        private readonly IRepository<Todo, TodoDataModel> _todoRepository;

        public Handler(IRepository<Todo, TodoDataModel> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoResultDTO> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.TodoCommandDTO;

            var todo = Todo.CreateNew(
                title: new(inputItem.Title!),
                description: !string.IsNullOrWhiteSpace(inputItem.Description) ?
                    new(inputItem.Description) : null,
                period: new(inputItem.BeginDateTime, inputItem.DueDateTime),
                state: inputItem.State != null ?
                    new((int)inputItem.State) : TodoState.Todo,
                ownerUserId: request.UserId
            );

            var result = await _todoRepository.CreateAsync(todo);
            return new TodoResultDTO(result);
        }
    }
}
