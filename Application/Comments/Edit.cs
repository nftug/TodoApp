using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using Persistence;
using AutoMapper;

namespace Application.Comments;

public class Edit
{
    public class Command : IRequest<Result<CommentDTO?>?>
    {
        public CommentDTO CommentDTO { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Command(Guid id, CommentDTO commentDTO, string userId)
        {
            CommentDTO = commentDTO;
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Command, Result<CommentDTO?>?>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<CommentDTO?>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.CommentDTO;

            if (request.Id != inputItem?.Id)
                return Result<CommentDTO?>.Failure("id", "Incorrect id");

            var item = await _context.Comments.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (item == null)
                return null;

            _mapper.Map(inputItem, item);

            await _context.SaveChangesAsync();

            var result = _mapper.Map<CommentDTO>(item);
            return Result<CommentDTO?>.Success(result);
        }
    }
}
