using MediatR;
using Pagination.EntityFrameworkCore.Extensions;
using Application.Shared.Interfaces;
using Domain.Shared.Entities;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Shared.Queries;

namespace Application.Shared.UseCases;

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
        protected readonly IDomainService<TDomain> _domain;

        public HandlerBase(
            IRepository<TDomain> repository,
            IDomainService<TDomain> domain
        )
        {
            _repository = repository;
            _domain = domain;
        }

        public virtual async Task<Pagination<TResultDTO>> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var queryParameter = _domain.GetQueryParameter(request.Param, request.UserId);

            var results = (await _repository
                .GetPaginatedListAsync(queryParameter))
                .Select(x => CreateDTO(x));

            var count = await _repository.GetCountAsync(queryParameter);

            return new Pagination<TResultDTO>
                (results, count, (int)queryParameter.Page!, (int)queryParameter.Limit!);
        }

        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}