using MediatR;
using Application.Shared.Interfaces;
using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Shared.UseCases;

public abstract class DetailsBase<TDomain, TResultDTO>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
{
    public class Query : IRequest<TResultDTO>
    {
        public Guid Id { get; init; }
        public Guid? UserId { get; init; }

        public Query(Guid id, Guid? userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Query, TResultDTO>
    {
        protected readonly IRepository<TDomain> _repository;
        protected readonly IDomainService<TDomain> _domain;

        public HandlerBase(IRepository<TDomain> repository, IDomainService<TDomain> domain)
        {
            _repository = repository;
            _domain = domain;
        }

        public virtual async Task<TResultDTO> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindAsync(request.Id);

            if (item == null)
                throw new NotFoundException();

            if (!await _domain.CanShow(item, request.UserId))
                throw new BadRequestException();

            return CreateDTO(item);
        }

        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}
