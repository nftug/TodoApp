using MediatR;
using Domain.Interfaces;
using Domain.Shared;
using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Shared.Interfaces;

namespace Application.Shared.UseCase;

public abstract class ListBase<TDomain, TEntity, TResultDTO>
    where TDomain : ModelBase
    where TEntity : IEntity
    where TResultDTO : IResultDTO<TDomain>
{
    public class Query : IRequest<Pagination<TResultDTO>>
    {
        public IQueryParameter<TEntity> Param { get; init; }
        public string? UserId { get; init; }

        public Query(IQueryParameter<TEntity> param, string? userId)
        {
            Param = param;
            UserId = userId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Query, Pagination<TResultDTO>>
    {
        private readonly IRepository<TDomain, TEntity> _repository;
        private readonly IQuerySearch<TEntity> _querySearch;

        public HandlerBase(
            IRepository<TDomain, TEntity> repository,
            IQuerySearch<TEntity> querySearch
        )
        {
            _repository = repository;
            _querySearch = querySearch;
        }

        public virtual async Task<Pagination<TResultDTO>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var filteredQuery = _querySearch
                .GetFilteredQuery(request.Param)
                .OrderByDescending(x => x.CreatedDateTime);

            var results = (await _repository
                .GetPaginatedListAsync(filteredQuery, request.Param))
                .Select(x => CreateDTO(x));

            var count = await filteredQuery.CountAsync();

            return new Pagination<TResultDTO>
                (results, count, request.Param.Page, request.Param.Limit);
        }

        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}