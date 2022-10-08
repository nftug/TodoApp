using MediatR;
using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Shared.DTOs;

namespace Application.Shared.UseCases;

public abstract class EditBase<TDomain, TResultDTO, TCommand>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
    where TCommand : ICommand<TDomain>
{
    public class Command : IRequest<TResultDTO>
    {
        public Guid Id { get; init; }
        public TCommand Item { get; init; }
        public Guid UserId { get; init; }

        public Command(Guid id, TCommand item, Guid userId)
        {
            Id = id;
            Item = item;
            UserId = userId;
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
            var item = await _repository.FindAsync(request.Id);
            if (item == null)
                throw new NotFoundException();

            Edit(item, request.Item);

            if (!await _domain.CanEdit(item, request.UserId))
                throw new ForbiddenException();

            var result = await _repository.UpdateAsync(item);

            if (result == null)
                throw new NotFoundException();

            return CreateDTO(result);
        }

        protected abstract void Edit(TDomain origin, TCommand command);

        protected abstract TResultDTO CreateDTO(TDomain item);
    }
}