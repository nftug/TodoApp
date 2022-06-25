using MediatR;
using Domain.Interfaces;
using Domain.Shared;
using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Shared.Interfaces;

namespace Application.Shared.UseCase;

public abstract class ListBase<TDomain, TResultDTO>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
{
    public class Query : IRequest<Pagination<TResultDTO>>
    {
        public IQueryParameter<TDomain> Param { get; init; }
        public Guid? UserId { get; init; }

        public Query(IQueryParameter<TDomain> param, Guid? userId)
        {
            Param = param;
            UserId = userId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Query, Pagination<TResultDTO>>
    {
        protected readonly IRepository<TDomain> _repository;
        protected readonly IQueryService<TDomain> _querySearch;

        public HandlerBase(
            IRepository<TDomain> repository,
            IQueryService<TDomain> querySearch
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
                .OrderByDescending(x => x.CreatedOn);

            var results = (await _repository
                .GetPaginatedListAsync(filteredQuery, request.Param))
                .Select(x => CreateDTO(x));

            var count = await filteredQuery.CountAsync(cancellationToken);

            return new Pagination<TResultDTO>
                (results, count, request.Param.Page, request.Param.Limit);
        }

        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}