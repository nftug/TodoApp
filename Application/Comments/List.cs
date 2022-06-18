using MediatR;
using Pagination.EntityFrameworkCore.Extensions;
using Infrastructure.Comments;
using Microsoft.EntityFrameworkCore;
using Domain.Comments;
using Infrastructure.DataModels;
using Domain.Shared;

namespace Application.Comments;

public class List
{
    public class Query : IRequest<Pagination<CommentResultDTO>>
    {
        public CommentQueryParameter Param { get; set; }
        public string? UserId { get; set; }

        public Query(CommentQueryParameter param, string? userId)
        {
            Param = param;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Query, Pagination<CommentResultDTO>>
    {
        private readonly CommentQuerySearchService _commentQuerySearchService;
        private readonly IRepository<Comment, CommentDataModel> _commentRepository;

        public Handler(
            IRepository<Comment, CommentDataModel> commentRepository,
            CommentQuerySearchService commentQuerySearchService
        )
        {
            _commentRepository = commentRepository;
            _commentQuerySearchService = commentQuerySearchService;
        }

        public async Task<Pagination<CommentResultDTO>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var filteredQuery = _commentQuerySearchService
                .GetFilteredQuery(request.Param)
                .OrderByDescending(x => x.CreatedDateTime);

            var results = (await _commentRepository
                .GetPaginatedListAsync(filteredQuery, request.Param))
                .Select(x => new CommentResultDTO(x));

            var count = await filteredQuery.CountAsync();

            return new Pagination<CommentResultDTO>
                (results, count, request.Param.Page, request.Param.Limit);
        }
    }
}
