#nullable disable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain;
using Persistence;

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest<CommentDTO>
        {
            public CommentDTO CommentDTO { get; set; }
        }

        public class Handler : IRequestHandler<Command, CommentDTO>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<CommentDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = request.CommentDTO.ToRawModel();
                item.CreatedAt = DateTime.Now;
                _context.Comments.Add(item);
                await _context.SaveChangesAsync();

                item = await _context.Comments.FirstOrDefaultAsync(x => x.Id == item.Id);

                return item.ToDTO();
            }
        }
    }
}