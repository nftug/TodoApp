using MediatR;
using Persistence;
using Pagination.EntityFrameworkCore.Extensions;
using Application.Comments.Query;
using Application.Core.Pagination;
using AutoMapper;

namespace Application.Comments;

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
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Pagination<CommentDTO>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var query = _context.Comments.GetFilteredQuery(request.Param);
            var results = _mapper.ProjectTo<CommentDTO>(query);

            return await results.GetPaginatedResultsAsync(request.Param);
        }
    }
}
