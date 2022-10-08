using MediatR;
using Domain.Shared.Exceptions;
using Domain.Shared.Entities;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Shared.DTOs;

namespace Application.Shared.UseCases;

public abstract class CreateBase<TDomain, TResultDTO, TCommand>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
    where TCommand : ICommand<TDomain>
{
    public class Command : IRequest<TResultDTO>
    {
        public TCommand Item { get; init; }
        public Guid UserId { get; init; }

        public Command(TCommand item, Guid usedId)
        {
            Item = item;
            UserId = usedId;
        }
    }

    public abstract class HandlerBase : IRequestHandler<Command, TResultDTO>
    {
        protected readonly IRepository<TDomain> _repository;
        protected readonly IDomainService<TDomain> _domain;

        public HandlerBase(IRepository<TDomain> repository, IDomainService<TDomain> domain)
        {
            _repository = repository;
            _domain = domain;
        }

        public virtual async Task<TResultDTO> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var item = CreateDomain(request);
            if (!await _domain.CanCreate(item, request.UserId))
                throw new ForbiddenException();

            var result = await _repository.CreateAsync(item);
            return CreateDTO(result);
        }

        protected abstract TDomain CreateDomain(Command request);
        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}
