using MediatR;
using Pagination.EntityFrameworkCore.Extensions;
using Infrastructure.Todos;
using Domain.Todos;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Infrastructure.DataModels;

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
        }
    }

    public class Handler : IRequestHandler<Query, Pagination<TodoResultDTO>>
    {
        private readonly TodoQuerySearchService _todoQuerySearchService;
        private readonly IRepository<Todo, TodoDataModel> _todoRepository;

        public Handler(
            IRepository<Todo, TodoDataModel> todoRepository,
            TodoQuerySearchService todoQuerySearchService
        )
        {
            _todoRepository = todoRepository;
            _todoQuerySearchService = todoQuerySearchService;
        }

        public async Task<Pagination<TodoResultDTO>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var filteredQuery = _todoQuerySearchService
                .GetFilteredQuery(request.Param)
                .OrderByDescending(x => x.CreatedDateTime);

            var results = (await _todoRepository
                .GetPaginatedListAsync(filteredQuery, request.Param))
                .Select(x => new TodoResultDTO(x));

            var count = await filteredQuery.CountAsync();

            return new Pagination<TodoResultDTO>
                (results, count, request.Param.Page, request.Param.Limit);
        }
    }
}
