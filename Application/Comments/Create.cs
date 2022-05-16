using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain;
using Persistence;
using AutoMapper;
using Application.Core;

namespace Application.Comments;

public class Create
{
    public class Command : IRequest<Result<CommentDTO?>>
    {
        public CommentDTO CommentDTO { get; set; }
        public string UserId { get; set; }

        public Command(CommentDTO commentDTO, string usedId)
        {
            CommentDTO = commentDTO;
            UserId = usedId;
        }
    }

    public class Handler : IRequestHandler<Command, Result<CommentDTO?>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<CommentDTO?>> Handle(Command request, CancellationToken cancellationToken)
        {
            var item = _mapper.Map<Comment>(request.CommentDTO);

            // 外部キーの存在判定
            if (await _context.TodoItems.FindAsync(item.TodoItemId) == null)
                return Result<CommentDTO?>.Failure("todoItemId", "Incorrect todoItemId");

            item.CreatedAt = DateTime.Now;

            _context.Comments.Add(item);
            await _context.SaveChangesAsync();

            var result = await _mapper.ProjectTo<CommentDTO>(_context.Comments)
                                .FirstOrDefaultAsync(x => x.Id == item.Id);

            return Result<CommentDTO?>.Success(result);
        }
    }
}
