using MediatR;
using Application.Core.Exceptions;
using Persistence;
using Domain;

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

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<CommentDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var todoItem = await _context.Comments.FindAsync(request.Id);

                if (todoItem == null)
                    throw new NotFoundException();

                return todoItem.ToDTO();
            }
        }
    }
}