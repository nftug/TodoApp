using MediatR;
using Pagination.EntityFrameworkCore.Extensions;
using Domain.Todos;
using Infrastructure.Todos;
using Domain.Comments;
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
            var filteredQuery = _todoQuerySearchService.GetFilteredQuery(request.Param);
            int page = (int)request.Param.Page!;
            int limit = (int)request.Param.Limit!;

            var paginatedQuery = filteredQuery.Skip((page - 1) * limit).Take(limit);

            var results = await paginatedQuery.Select(
                x => TodoResultDTO.CreateResultDTO(Todo.CreateFromRepository(
                        x.Id,
                        new TodoTitle(x.Title),
                        !string.IsNullOrWhiteSpace(x.Description)
                            ? new TodoDescription(x.Description) : null,
                        x.BeginDateTime,
                        x.DueDateTime,
                        new TodoState(x.State),
                        x.Comments.Select(x =>
                            Comment.CreateFromRepository(
                                x.Id,
                                new CommentContent(x.Content),
                                x.TodoId,
                                x.CreatedDateTime,
                                x.UpdatedDateTime,
                                x.OwnerUserId
                            )).ToList(),
                        x.CreatedDateTime,
                        x.UpdatedDateTime,
                        x.OwnerUserId
                    ))
                ).ToListAsync();

            var count = await filteredQuery.CountAsync();

            return new Pagination<TodoResultDTO>(results, count, page, limit);
        }
    }
}