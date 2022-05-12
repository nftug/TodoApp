#nullable disable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Core.Exceptions;
using Persistence;
using Domain;

namespace Application.Comments
{
    public class Edit
    {
        public class Command : IRequest<CommentDTO>
        {
            public Comment Comment { get; set; }
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommentDTO>
        {
            private readonly TodoContext _context;

            public Handler(TodoContext context)
            {
                _context = context;
            }

            public async Task<CommentDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                if (request.Id != request.Comment.Id)
                    throw new BadRequestException();

                _context.Entry(request.Comment).State = EntityState.Modified;

                var item = await _context.Comments.FindAsync(request.Id);
                if (item == null)
                    throw new NotFoundException();

                request.Comment.CreatedAt = item.CreatedAt;

                await _context.SaveChangesAsync();

                return item.ItemToDTO();
            }
        }
    }
}