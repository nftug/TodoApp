using MediatR;
using Persistence;
using Pagination.EntityFrameworkCore.Extensions;
using Application.TodoItems.Query;
using Application.Core.Query;
using AutoMapper;

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
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Pagination<TodoItemDTO>> Handle
                (Query request, CancellationToken cancellationToken)
            {
                var query = _context.TodoItems.GetFilteredQuery(request.Param);
                var results = _mapper.ProjectTo<TodoItemDTO>(query);

                return await results.GetPaginatedResultsAsync(request.Param);
            }
        }
    }
}