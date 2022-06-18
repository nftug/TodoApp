using MediatR;
using Domain.Todos;
using Domain.Shared;
using Infrastructure.DataModels;

namespace Application.Todos;

public class Details
{
    public class Query : IRequest<TodoResultDTO>
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }

        public Query(Guid id, string? userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Query, TodoResultDTO>
    {
        private readonly IRepository<Todo, TodoDataModel> _todoRepository;

        public Handler(IRepository<Todo, TodoDataModel> todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<TodoResultDTO> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var todo = await _todoRepository.FindAsync(request.Id);

            if (todo == null)
                throw new NotFoundException();

            return new TodoResultDTO(todo);
        }
    }
}
