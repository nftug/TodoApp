using MediatR;
using Pagination.EntityFrameworkCore.Extensions;
using Infrastructure.Todos;
using Domain.Todos;
using Application.Comments;
using Microsoft.EntityFrameworkCore;

namespace Application.Todos;

public class List
{
    public class Query : IRequest<Pagination<TodoResultDTO>>
    {
        public TodoQueryParameter Param { get; set; }
        public string? UserId { get; set; }

        public Query(TodoQueryParameter param, string? userId)
        {
            Param = param;
            UserId = userId;
            Param.Page ??= 1;
            Param.Limit ??= 10;
        }
    }

    public class Handler : IRequestHandler<Query, Pagination<TodoResultDTO>>
    {
        private readonly TodoQuerySearchService _todoQuerySearchService;

        public Handler(TodoQuerySearchService todoQuerySearchService)
        {
            _todoQuerySearchService = todoQuerySearchService;
        }

        public async Task<Pagination<TodoResultDTO>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var filteredQuery = _todoQuerySearchService.GetFilteredQuery(request.Param)
                                                       .OrderByDescending(x => x.CreatedDateTime);
            int page = (int)request.Param.Page!;
            int limit = (int)request.Param.Limit!;

            var paginatedQuery = filteredQuery.Skip((page - 1) * limit).Take(limit);

            var results = await paginatedQuery.Select(
                x => new TodoResultDTO(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.BeginDateTime,
                    x.DueDateTime,
                    new TodoState(x.State),
                    x.Comments.Select(
                        _ => new CommentResultDTO(
                                _.Id,
                                _.Content,
                                _.TodoId,
                                _.CreatedDateTime,
                                _.UpdatedDateTime,
                                _.OwnerUserId
                            )
                        ).ToList(),
                    x.CreatedDateTime,
                    x.UpdatedDateTime,
                    x.OwnerUserId
                )
            ).ToListAsync();

            var count = await filteredQuery.CountAsync();

            return new Pagination<TodoResultDTO>(results, count, page, limit);
        }
    }
}
