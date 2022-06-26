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
        protected readonly IDomainService<TDomain> _domain;

        public HandlerBase(
            IRepository<TDomain> repository,
            IQueryService<TDomain> querySearch,
            IDomainService<TDomain> domain
        )
        {
            _repository = repository;
            _querySearch = querySearch;
            _domain = domain;
        }

        public virtual async Task<Pagination<TResultDTO>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var queryParameter = _domain.GetQueryParameter(request.Param, request.UserId);

            var filteredQuery = _querySearch.GetFilteredQuery(queryParameter);

            var results = (await _repository
                .GetPaginatedListAsync(filteredQuery, queryParameter))
                .Select(x => CreateDTO(x));

            var count = await filteredQuery.CountAsync(cancellationToken);

            return new Pagination<TResultDTO>
                (results, count, queryParameter.Page, queryParameter.Limit);
        }

        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}