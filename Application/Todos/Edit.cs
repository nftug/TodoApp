using MediatR;
using Domain.Todos;
using Domain.Shared;
using Infrastructure.DataModels;

namespace Application.Todos;

public class Edit
{
    public class Command : IRequest<TodoResultDTO>
    {
        public TodoCommandDTO TodoCommandDTO { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Command(Guid id, TodoCommandDTO todoItemDTO, string userId)
        {
            TodoCommandDTO = todoItemDTO;
            Id = id;
            UserId = userId;
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

            if (request.Id != inputItem.Id)
                throw new DomainException(nameof(inputItem.Id), "IDが正しくありません");

            var todo = await _todoRepository.FindAsync(request.Id);
            if (todo == null)
                throw new NotFoundException();
            if (todo.OwnerUserId != request.UserId)
                throw new BadRequestException();

            todo.Edit(
                title: new(inputItem.Title!),
                description: !string.IsNullOrEmpty(inputItem.Description)
                    ? new(inputItem.Description) : null,
                period: new(inputItem.BeginDateTime, inputItem.DueDateTime),
                state: inputItem.State != null
                    ? new((int)inputItem.State) : TodoState.Todo
            );

            var result = await _todoRepository.UpdateAsync(todo);

            return new TodoResultDTO(result);
        }
    }
}
