using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Todos;
using Persistence;
using AutoMapper;
using Application.Core;

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
            var todo = Todo.CreateNew(
                title: new TodoTitle(request.TodoCommandDTO.Title),
                description: request.TodoCommandDTO.Description != null ?
                    new TodoDescription(request.TodoCommandDTO.Description) : null,
                beginDateTime: request.TodoCommandDTO.BeginDateTime,
                dueDateTime: request.TodoCommandDTO.DueDateTime,
                state: request.TodoCommandDTO.State != null ?
                    new TodoState((int)request.TodoCommandDTO.State) : TodoState.Todo,
                ownerUserId: request.UserId
            );

            var result = await _todoRepository.CreateAsync(todo);
            return TodoResultDTO.CreateResultDTO(result);
        }
    }
}
