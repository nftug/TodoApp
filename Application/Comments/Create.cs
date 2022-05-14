#nullable disable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain;
using Persistence;
using AutoMapper;
using Application.Core.Exceptions;

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
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CommentDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = _mapper.Map<Comment>(request.CommentDTO);

                // 外部キーの存在判定
                if (await _context.TodoItems.FindAsync(item.TodoItemId) == null)
                    throw new BadRequestException();

                item.CreatedAt = DateTime.Now;
                _context.Comments.Add(item);
                await _context.SaveChangesAsync();

                return await _mapper.ProjectTo<CommentDTO>(_context.Comments)
                                    .FirstOrDefaultAsync(x => x.Id == item.Id);
            }
        }
    }
}