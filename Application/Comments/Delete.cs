using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;

namespace Application.Comments;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Command(Guid id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var Comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (Comment == null)
                return Result<Unit>.NotFound();

            _context.Comments.Remove(Comment);
            await _context.SaveChangesAsync();

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
