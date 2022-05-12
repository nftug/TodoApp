#nullable disable
using MediatR;
using Domain;
using Persistence;

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest
        {
            public Comment Comment { get; set; }
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
                request.Comment.CreatedAt = DateTime.Now;
                _context.Comments.Add(request.Comment);
                await _context.SaveChangesAsync();

                return Unit.Value;
            }
        }
    }
}