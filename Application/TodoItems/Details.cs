using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.Core;

namespace Application.TodoItems;

public class Details
{
    public class Query : IRequest<Result<TodoItemDTO?>>
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }

        public Query(Guid id, string? userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Query, Result<TodoItemDTO?>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<TodoItemDTO?>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _mapper.ProjectTo<TodoItemDTO>(_context.TodoItems)
                                      .FirstOrDefaultAsync(x => x.Id == request.Id);

            return Result<TodoItemDTO?>.Success(result);
        }
    }
}
