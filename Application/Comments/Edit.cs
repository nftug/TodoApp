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
            public CommentDTO CommentDTO { get; set; }
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
                var inputItem = request.CommentDTO.ToRawModel();

                if (request.Id != inputItem.Id)
                    throw new BadRequestException();

                _context.Entry(inputItem).State = EntityState.Modified;

                var item = await _context.Comments.FindAsync(request.Id);
                if (item == null)
                    throw new NotFoundException();

                inputItem.CreatedAt = item.CreatedAt;

                await _context.SaveChangesAsync();

                return item.ToDTO();
            }
        }
    }
}