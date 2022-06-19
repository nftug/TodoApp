using MediatR;
using Domain.Interfaces;
using Domain.Shared;
using Application.Shared.Interfaces;

namespace Application.Shared.UseCase;

public abstract class DetailsBase<TDomain, TEntity, TResultDTO>
    where TDomain : ModelBase
    where TEntity : IEntity
    where TResultDTO : IResultDTO<TDomain>
{
    public class Query : IRequest<TResultDTO>
    {
        public Guid Id { get; init; }
        public string? UserId { get; init; }

        public Query(Guid id, string? userId)
        {
            Id = id;
            UserId = userId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Query, TResultDTO>
    {
        private readonly IRepository<TDomain, TEntity> _repository;

        public HandlerBase(IRepository<TDomain, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<TResultDTO> Handle
            (Query request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindAsync(request.Id);

            if (item == null)
                throw new NotFoundException();

            return CreateDTO(item);
        }

        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}
