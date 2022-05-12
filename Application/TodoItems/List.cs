using MediatR;
using Domain;
using Persistence;
using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.TodoItems
{
    public class List
    {
        public class Query : IRequest<Pagination<TodoItemDTO>>
        {
            public int Page { get; set; }
            public int Limit { get; set; }
        }

        public class Handler : IRequestHandler<Query, Pagination<TodoItemDTO>>
        {
            private readonly TodoContext _context;

            public Handler(TodoContext context)
            {
                _context = context;
            }

            public async Task<Pagination<TodoItemDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.TodoItems;

                var results = await query
                                    .Select(x => x.ItemToDTO())
                                    .Skip((request.Page - 1) * request.Limit).Take(request.Limit)
                                    .ToListAsync();
                var count = query.Count();

                return new Pagination<TodoItemDTO>(results, count, request.Page, request.Limit);
            }
        }
    }
}