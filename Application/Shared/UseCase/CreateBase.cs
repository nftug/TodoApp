using MediatR;
using Domain.Interfaces;
using Domain.Shared;
using Application.Shared.Interfaces;

namespace Application.Shared.UseCase;

public abstract class CreateBase<TDomain, TResultDTO, TCommandDTO>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
    where TCommandDTO : ICommandDTO<TDomain>
{
    public class Command : IRequest<TResultDTO>
    {
        public TCommandDTO Item { get; init; }
        public Guid UserId { get; init; }

        public Command(TCommandDTO item, Guid usedId)
        {
            Item = item;
            UserId = usedId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Command, TResultDTO>
    {
        protected readonly IRepository<TDomain> _repository;

        public HandlerBase(IRepository<TDomain> repository)
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
