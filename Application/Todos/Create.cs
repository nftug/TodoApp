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
                description: new(inputItem.Description),
                period: new(inputItem.BeginDateTime, inputItem.DueDateTime),
                state: new(inputItem.State),
                ownerUserId: request.UserId
            );

            var result = await _todoRepository.CreateAsync(todo);
            return new TodoResultDTO(result);
        }
    }
}
