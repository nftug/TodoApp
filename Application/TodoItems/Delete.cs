using MediatR;
using Application.Core.Exceptions;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.TodoItems;

public class Delete
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var todoItem = await _context.TodoItems
                                         .FirstOrDefaultAsync(
                                             x => x.Id == request.Id &&
                                             x.CreatedById == request.UserId
                                         );

            if (todoItem == null)
                throw new NotFoundException();

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
