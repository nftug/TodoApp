using MediatR;
using Domain.Interfaces;
using Application.Shared.Interfaces;
using Domain.Shared.Exceptions;
using Domain.Shared.Entities;

namespace Application.Shared.UseCases;

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
        protected readonly IDomainService<TDomain> _domain;

        public HandlerBase(
            IRepository<TDomain> repository,
            IDomainService<TDomain> domain
        )
        {
            _repository = repository;
            _domain = domain;
        }

        public virtual async Task<TResultDTO> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var item = CreateDomain(request);
            if (!await _domain.CanCreate(item, request.UserId))
                throw new BadRequestException();

            var result = await _repository.CreateAsync(item);
            return CreateDTO(result);
        }

        protected abstract TDomain CreateDomain(Command request);
        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}
