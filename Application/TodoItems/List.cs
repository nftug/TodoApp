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
            public string UserId { get; set; }

            public Query(QueryParameter param, string userId)
            {
                Param = param;
                Param.Page ??= 1;
                Param.Limit ??= 10;
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, Pagination<TodoItemDTO>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
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