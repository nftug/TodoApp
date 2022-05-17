using MediatR;
using Pagination.EntityFrameworkCore.Extensions;
using Domain.Comments;
using Infrastructure.Comments;
using Microsoft.EntityFrameworkCore;

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
            Param.Page ??= 1;
            Param.Limit ??= 10;
        }
    }

    public class Handler : IRequestHandler<Query, Pagination<CommentResultDTO>>
    {
        private readonly CommentQuerySearchService _commentQuerySearchService;

        public Handler(CommentQuerySearchService commentQuerySearchService)
        {
            _commentQuerySearchService = commentQuerySearchService;
        }

        public async Task<Pagination<CommentResultDTO>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var filteredQuery = _commentQuerySearchService.GetFilteredQuery(request.Param);
            int page = (int)request.Param.Page!;
            int limit = (int)request.Param.Limit!;

            var paginatedQuery = filteredQuery.Skip((page - 1) * limit).Take(limit);

            var results = await paginatedQuery.Select(
                x => CommentResultDTO.CreateResultDTO(Comment.CreateFromRepository(
                        x.Id,
                        new CommentContent(x.Content),
                        x.TodoId,
                        x.CreatedDateTime,
                        x.UpdatedDateTime,
                        x.OwnerUserId
                    ))
                ).ToListAsync();

            var count = await filteredQuery.CountAsync();

            return new Pagination<CommentResultDTO>(results, count, page, limit);
        }
    }
}
