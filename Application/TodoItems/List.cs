using MediatR;
using Domain;
using Persistence;
using Pagination.EntityFrameworkCore.Extensions;
using Application.TodoItems.Query;

namespace Application.TodoItems
{
    public class List
    {
        public class Query : IRequest<Pagination<TodoItemDTO>>
        {
            public QueryParameter Param { get; set; } = new QueryParameter();

            public Query(QueryParameter param)
            {
                Param = param;
                Param.Page ??= 1;
                Param.Limit ??= 10;
            }
        }

        public class Handler : IRequestHandler<Query, Pagination<TodoItemDTO>>
        {
            private readonly TodoContext _context;

            public Handler(TodoContext context)
            {
                _context = context;
            }

            public async Task<Pagination<TodoItemDTO>> Handle
                (Query request, CancellationToken cancellationToken)
            {
                var query = _context.TodoItems.AsQueryable();
                return await query.GetQueryResultsAsync(request.Param);
            }
        }
    }
}