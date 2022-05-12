using MediatR;
using Application.Core.Exceptions;
using Persistence;

namespace Application.TodoItems
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly TodoContext _context;

            public Handler(TodoContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var todoItem = await _context.TodoItems.FindAsync(request.Id);
                if (todoItem == null)
                    throw new NotFoundException();

                _context.TodoItems.Remove(todoItem);
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}