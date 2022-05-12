using MediatR;
using Domain;
using Persistence;
using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Comments.Query;

namespace Application.Comments
{
    public class List
    {
        public class Query : IRequest<Pagination<CommentDTO>>
        {
            public QueryParameter Param { get; set; } = new QueryParameter();

            public Query(QueryParameter param)
            {
                Param = param;
                Param.Page ??= 1;
                Param.Limit ??= 10;
            }
        }

        public class Handler : IRequestHandler<Query, Pagination<CommentDTO>>
        {
            private readonly TodoContext _context;

            public Handler(TodoContext context)
            {
                _context = context;
            }

            public async Task<Pagination<CommentDTO>> Handle
                (Query request, CancellationToken cancellationToken)
            {
                var query = _context.Comments as IQueryable<Comment>;
                return await query.GetQueryResultsAsync(request.Param);
            }
        }
    }
}