using MediatR;
using Domain.Interfaces;
using Domain.Shared;
using Application.Shared.Interfaces;

namespace Application.Shared.UseCase;

public abstract class CreateBase<TDomain, TEntity, TResultDTO, TCommandDTO>
    where TDomain : ModelBase
    where TEntity : IEntity
    where TResultDTO : IResultDTO<TDomain>
    where TCommandDTO : ICommandDTO<TDomain>
{
    public class Command : IRequest<TResultDTO>
    {
        public TCommandDTO Item { get; set; }
        public string UserId { get; set; }

        public Command(TCommandDTO item, string usedId)
        {
            Item = item;
            UserId = usedId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Command, TResultDTO>
    {
        private readonly IRepository<TDomain, TEntity> _repository;

        public HandlerBase(IRepository<TDomain, TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<TResultDTO> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var item = CreateDomain(request);

            var result = await _repository.CreateAsync(item);
            return CreateDTO(result);
        }

        protected abstract TDomain CreateDomain(Command request);
        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}
