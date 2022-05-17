using MediatR;
using Domain.Todos;
using Domain.Shared;

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
        private readonly ITodoRepository _todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoResultDTO> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Id != request.TodoCommandDTO.Id)
                throw new DomainException("id", "IDが正しくありません");

            var todo = await _todoRepository.FindAsync(request.Id);
            if (todo == null)
                throw new NotFoundException();
            if (todo.OwnerUserId != request.UserId)
                throw new BadRequestException();

            todo.Edit(
                title: new TodoTitle(request.TodoCommandDTO.Title),
                description: request.TodoCommandDTO.Description != null ?
                    new TodoDescription(request.TodoCommandDTO.Description) : null,
                beginDateTime: request.TodoCommandDTO.BeginDateTime,
                dueDateTime: request.TodoCommandDTO.DueDateTime,
                state: request.TodoCommandDTO.State != null ?
                    new TodoState((int)request.TodoCommandDTO.State) : TodoState.Todo
            );

            var result = await _todoRepository.UpdateAsync(todo);

            return TodoResultDTO.CreateResultDTO(result);
        }
    }
}
