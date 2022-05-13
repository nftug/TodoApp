using MediatR;
using Application.Core.Exceptions;
using Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

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

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<TodoItemDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var todoItem = await _context.TodoItems
                                             .Include(x => x.Comments)
                                             .FirstOrDefaultAsync(x => x.Id == request.Id);

                if (todoItem == null)
                    throw new NotFoundException();

                return todoItem.ToDTO();
            }
        }
    }
}