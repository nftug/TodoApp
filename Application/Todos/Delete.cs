using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using Domain.Todos;
using Domain.Shared;

namespace Application.Todos;

public class Delete
{
    public class Command : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Command(Guid id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly ITodoRepository _todoRepository;

        public Handler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var todoItem = await _todoRepository.FindAsync(request.Id);

            if (todoItem == null)
                throw new NotFoundException();
            if (todoItem.OwnerUserId != request.UserId)
                throw new BadRequestException();

            await _todoRepository.RemoveAsync(request.Id);

            return Unit.Value;
        }
    }
}
