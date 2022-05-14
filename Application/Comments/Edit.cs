#nullable disable
using MediatR;
using Application.Core.Exceptions;
using Persistence;
using AutoMapper;

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
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CommentDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var inputItem = request.CommentDTO;

                if (request.Id != inputItem.Id)
                    throw new BadRequestException();

                var item = await _context.Comments.FindAsync(request.Id);
                if (item == null)
                    throw new NotFoundException();

                _mapper.Map(inputItem, item);

                await _context.SaveChangesAsync();

                return _mapper.Map<CommentDTO>(item);
            }
        }
    }
}