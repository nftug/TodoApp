using MediatR;
using Application.Core.Exceptions;
using Persistence;

namespace Application.Comments;

public class Delete
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
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
            var todoItem = await _context.Comments.FindAsync(request.Id);
            if (todoItem == null)
                throw new NotFoundException();

            _context.Comments.Remove(todoItem);
            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
