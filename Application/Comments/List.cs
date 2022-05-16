using MediatR;
using Persistence;
using Pagination.EntityFrameworkCore.Extensions;
using Application.Comments.Query;
using Application.Core.Pagination;
using Application.Core;
using AutoMapper;

namespace Application.Comments;

public class List
{
    public class Query : IRequest<Result<Pagination<CommentDTO>>>
    {
        public QueryParameter Param { get; set; }
        public string? UserId { get; set; }

        public Query(QueryParameter param, string? userId)
        {
            Param = param;
            Param.Page ??= 1;
            Param.Limit ??= 10;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Query, Result<Pagination<CommentDTO>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Pagination<CommentDTO>>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var query = _context.Comments.GetFilteredQuery(request.Param);
            var results = _mapper.ProjectTo<CommentDTO>(query);

            var paginatedResults = await results.GetPaginatedResultsAsync(request.Param);
            return Result<Pagination<CommentDTO>>.Success(paginatedResults);
        }
    }
}
