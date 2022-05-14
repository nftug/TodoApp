using MediatR;
using Application.Core.Exceptions;
using Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Comments
{
    public class Details
    {
        public class Query : IRequest<CommentDTO>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, CommentDTO>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CommentDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _mapper.ProjectTo<CommentDTO>(_context.Comments)
                                          .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (result == null)
                    throw new NotFoundException();

                return result;
            }
        }
    }
}