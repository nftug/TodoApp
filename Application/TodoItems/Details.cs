using MediatR;
using Application.Core.Exceptions;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.TodoItems
{
    public class Details
    {
        public class Query : IRequest<TodoItemDTO>
        {
            public Guid Id { get; set; }
            public string? UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, TodoItemDTO>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TodoItemDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = await _mapper.ProjectTo<TodoItemDTO>(_context.TodoItems)
                                          .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (result == null)
                    throw new NotFoundException();

                return result;
            }
        }
    }
}